﻿
using Microsoft.AspNetCore.Identity;
using Application.DTOs;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<AuthResult> RegisterAsync(string username, string password);
    Task<AuthResult> LoginAsync(string username, string password);
    Task<AuthResult> RefreshTokenAsync(string token, string refreshToken);
    Task<AuthResult> LoginWithGoogleAsync(string idToken);
    Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<bool> ResetPasswordAsync(string email);
    Task<IdentityUser?> GetUserByIdAsync(string userId);
    Task<bool> AssignRoleAsync(string userId, string role);
}