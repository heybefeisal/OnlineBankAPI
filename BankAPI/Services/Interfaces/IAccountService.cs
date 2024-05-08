using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;

namespace BankAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(int userId, AccountRequestDto accountRequestDto);
        Task<Transaction> DepositAsync(int accountId, WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto);
        Task<Transaction> WithdrawlAsync(int accountId, WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto);
    }
}
