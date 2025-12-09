using Domain.Entities;

namespace Application.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(string userId);
    Task AddAsync(UserProfile profile);
    Task UpdateAsync(UserProfile profile);
    Task<(List<UserProfile> Items, int TotalCount)> GetPagedAsync(int page, int size);
}