using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;

namespace BankAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(int userId, AccountRequestDto accountRequestDto);

        //Task<Account> AccountExistsAsync(string accountNumber);
    }
}
