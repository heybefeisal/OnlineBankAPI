using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;

namespace BankAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<String> Authenticate(string username, string password);
        Task<User> Register(RegisterRequestDto registerRequestDto);
    }
}
