// Application/Interfaces/IUserRepository.cs
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user, string password);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(User user);
        Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ValidateUserCredentialsAsync(string userNameOrEmail, string password);
        Task<bool> AddUserToRoleAsync(User user, string role);
        Task<bool> RemoveUserFromRoleAsync(User user, string role);
        Task<IList<string>> GetUserRolesAsync(User user);
    }
}