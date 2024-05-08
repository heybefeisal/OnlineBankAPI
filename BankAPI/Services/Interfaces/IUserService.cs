using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;

namespace BankAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> CreateUserAsync(UserRequestDto userRequestDto);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> UpdateUserAsync(int userId, UserRequestDto userRequestDto);
        Task<bool> DeleteUser(int userId);
    }
}
