using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;


namespace BankAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> DepositMoneyAsync(int accountId,WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto);

        Task<Transaction> WithdrawlMoneyAsync(int accountId, WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto);

        Task<Transaction> TransferMoneyAsync(int FromAccountId, TransferRequestDto transactionRequestDto);

    }
}
