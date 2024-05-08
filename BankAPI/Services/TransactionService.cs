using AutoMapper;
using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Transaction = BankAPI.Models.Transaction;

namespace BankAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankDbContext _bankDbContext;

        public TransactionService(BankDbContext bankDbContext)
        {
            _bankDbContext = bankDbContext;
        }
        public async Task<Transaction> DepositMoneyAsync(int accountId, WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto)
        {
            var account = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);
            if(account == null) 
            {
                return null;
            }
            
            var deposit = new Transaction
            {
                ToAccountId = accountId,
                FromAccountId = accountId,
                Amount = withdrawlOrDepositRequestDto.Amount,
                TransactionDate = DateTime.UtcNow,
            };

            account.Balance += withdrawlOrDepositRequestDto.Amount;
            _bankDbContext.Entry(account).State = EntityState.Modified;

            await _bankDbContext.AddAsync(deposit);
            await _bankDbContext.SaveChangesAsync();
            return deposit;
        }

        public async Task<Transaction> WithdrawlMoneyAsync(int accountId, WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto)
        {
            var account = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);
            if (account == null)
            {
                return null;
            }

            var withdrawl = new Transaction
            {
                ToAccountId = accountId,
                FromAccountId = accountId,
                Amount = withdrawlOrDepositRequestDto.Amount,
                TransactionDate = DateTime.UtcNow,
            };

            account.Balance -= withdrawlOrDepositRequestDto.Amount;
            _bankDbContext.Entry(account).State = EntityState.Modified;

            await _bankDbContext.AddAsync(withdrawl);
            await _bankDbContext.SaveChangesAsync();
            return withdrawl;
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
