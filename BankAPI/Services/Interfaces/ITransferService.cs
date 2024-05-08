using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;


namespace BankAPI.Services.Interfaces
{
    public interface ITransferService
    {
        Task<Transaction> TransferAsync(int FromAccountId, TransferRequestDto transactionRequestDto);

    }
}
