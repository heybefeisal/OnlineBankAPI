using AutoMapper;
using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Transaction = BankAPI.Models.Transaction;

namespace BankAPI.Services
{
    public class TransferService : ITransferService
    {
        private readonly BankDbContext _bankDbContext;

        public TransferService(BankDbContext bankDbContext)
        {
            _bankDbContext = bankDbContext;
        }
        
        public async Task<Transaction> TransferMoneyAsync(int fromAccountId, TransferRequestDto transferRequestDto)
        {
            var fromAccount = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == fromAccountId); 

            if (fromAccount == null)
            {
                return null; 
            }

            if (fromAccount.Balance < transferRequestDto.Amount)
            {
                return null; 
            }

            var toAccount = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == transferRequestDto.ToAccountId); 

            if (toAccount == null)
            {
                return null; 
            }

            var transfer = new Transaction 
            {
                FromAccountId = fromAccountId,
                ToAccountId = transferRequestDto.ToAccountId,
                Amount = transferRequestDto.Amount,
                TransactionDate = DateTime.UtcNow,
            };

            fromAccount.Balance -= transfer.Amount; 

            _bankDbContext.Entry(fromAccount).State = EntityState.Modified;
            await _bankDbContext.AddAsync(transfer);
            await _bankDbContext.SaveChangesAsync();

            toAccount.Balance += transferRequestDto.Amount;
            _bankDbContext.Entry(toAccount).State = EntityState.Modified;

            try
            {
                await _bankDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Concurrency exception: {ex.Message}");
                return null;
            }

            return transfer;
        }
    }
}
