using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Google.Apis.Auth;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using Persistence.Contexts;

namespace Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly GoogleAuthSettings _googleAuthSettings;
    private readonly IUserProfileRepository _userProfileRepository;

    public IdentityService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        AppDbContext context,
        IOptions<GoogleAuthSettings> googleAuthSettings,
        IUserProfileRepository userProfileRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
        _googleAuthSettings = googleAuthSettings.Value;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<AuthResult> RegisterAsync(string email, string password)
    {
        var user = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Customer");
            var jwtSecurityToken = await GenerateJwtToken(user);
            var tokenHandler = new JwtSecurityTokenHandler();

            return new AuthResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(jwtSecurityToken),
                UserId = user.Id,
                Email = user.Email!,
                Roles = new List<string> { "Customer" }
            };
        }

        return new AuthResult
        {
            Success = false,
            Message = string.Join(", ", result.Errors.Select(e => e.Description))
        };
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return new AuthResult { Success = false, Message = "Invalid credentials" };

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
            return new AuthResult { Success = false, Message = "Invalid credentials" };

        var roles = await _userManager.GetRolesAsync(user);
        var token = await GenerateJwtToken(user);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.WriteToken(token);

        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            JwtId = token.Id, // Store the JWT ID
            UserId = user.Id,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return new AuthResult
        {
            Success = true,
            Token = jwt,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Email = user.Email!,
            Roles = roles.ToList(),
            Expiration = token.ValidTo
        };
    }

    public async Task<AuthResult> LoginWithGoogleAsync(string idToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleAuthSettings.ClientId }
            });

            var user = await _userManager.FindByLoginAsync("Google", payload.Subject);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    // Create a new user
                    user = new IdentityUser
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        EmailConfirmed = payload.EmailVerified
                    };
                    var createUserResult = await _userManager.CreateAsync(user);
                    if (!createUserResult.Succeeded)
                    {
                        return new AuthResult { Success = false, Message = "Failed to create user." };
                    }

                    await _userManager.AddToRoleAsync(user, "Customer");

                    // Create user profile
                    var profile = new UserProfile
                    {
                        UserId = user.Id,
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _userProfileRepository.AddAsync(profile);
                }

                // Add Google login to the user
                var logins = await _userManager.GetLoginsAsync(user);
                if (!logins.Any(l => l.LoginProvider == "Google" && l.ProviderKey == payload.Subject))
                {
                    var addLoginResult = await _userManager.AddLoginAsync(user,
                        new UserLoginInfo("Google", payload.Subject, "Google"));
                    if (!addLoginResult.Succeeded)
                    {
                        return new AuthResult { Success = false, Message = "Failed to add Google login." };
                    }
                }

            }

            // At this point, user exists and has a Google login. Generate tokens.
            var roles = await _userManager.GetRolesAsync(user);
            var token = await GenerateJwtToken(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            await SaveRefreshToken(user.Id, token.Id, refreshToken);

            return CreateAuthResult(user, jwt, refreshToken, roles.ToList(), token.ValidTo);
        }
        catch (InvalidJwtException ex)
        {
            return new AuthResult { Success = false, Message = $"Invalid Google token: {ex.Message}" };
        }
    }

    public async Task<AuthResult> RefreshTokenAsync(string token, string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateLifetime = false, // We don't care if the token is expired here
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            var jwtSecurityToken = validatedToken as JwtSecurityToken;

            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return new AuthResult { Success = false, Message = "Invalid token." };
            }

            var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jti == null)
            {
                return new AuthResult { Success = false, Message = "Invalid token." };
            }

            var storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken is null || DateTime.UtcNow > storedRefreshToken.ExpiryDate || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked || storedRefreshToken.JwtId != jti)
            {
                return new AuthResult { Success = false, Message = "Invalid refresh token." };
            }

            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var userId = principal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            var user = await _userManager.FindByIdAsync(userId!);
            if (user is null)
            {
                return new AuthResult { Success = false, Message = "User not found." };
            }

            var newJwtToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                JwtId = newJwtToken.Id,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            await _context.RefreshTokens.AddAsync(newRefreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(newJwtToken),
                RefreshToken = newRefreshToken,
                Expiration = newJwtToken.ValidTo
            };
        }
        catch (Exception)
        {
            return new AuthResult { Success = false, Message = "Invalid token." };
        }
    }

    public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.Succeeded;
    }

    public async Task<bool> ResetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // Here you would send email with reset link
        return true;
    }

    public async Task<IdentityUser?> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<bool> AssignRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }

    private async Task SaveRefreshToken(string userId, string jwtId, string token)
    {
        var refreshTokenEntity = new RefreshToken
        {
            Token = token,
            JwtId = jwtId,
            UserId = userId,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();
    }

    private AuthResult CreateAuthResult(IdentityUser user, string token, string refreshToken, List<string> roles, DateTime expiration)
    {
        return new AuthResult
        {
            Success = true,
            Token = token,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Email = user.Email!,
            Roles = roles,
            Expiration = expiration
        };
    }
    private async Task<JwtSecurityToken> GenerateJwtToken(IdentityUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);
        var jwtId = Guid.NewGuid().ToString(); // Generate a unique JWT ID
        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(ClaimTypes.Email, user.Email!),
        new(JwtRegisteredClaimNames.Jti, jwtId) // Add the JWT ID claim
    };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: credentials
        );

        return token;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
