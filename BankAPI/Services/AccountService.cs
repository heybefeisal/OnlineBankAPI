using AutoMapper;
using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly BankDbContext _bankDbContext;
        private readonly IUserService _userService;

        public AccountService(IMapper mapper, BankDbContext bankDbContext, IUserService userService)
        {
            _mapper = mapper;
            _bankDbContext = bankDbContext;
            _userService = userService;
        }

        public async Task<Decimal?> GetBalanceAsync(int userId, int accountId)
        {
            var account = await _bankDbContext.Accounts.FirstOrDefaultAsync( x=> x.UserId == userId && x.AccountId == accountId);

            if (account == null)
            {
                return null;
            }

            var balance = account.Balance;

            if (balance == null)
            {
                return null;
            }

            return balance;
            
        }

        public async Task<Account> CreateAccountAsync(int userId, AccountRequestDto accountRequestDto)
        {
            var user = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.UserId == userId);

            if(user == null) 
            {
                return null;
            }

            if (await UserHasPrivateAccount(userId))
            {
                throw new Exception("Private account exists");
            }

            var account = new Account
            {
                UserId = userId,
                AccountNumber = accountRequestDto.AccountNumber,
                AccountType = accountRequestDto.AccountType,
                Balance = accountRequestDto.Balance,
            };

            await _bankDbContext.Accounts.AddAsync(account);
            await _bankDbContext.SaveChangesAsync();
            return account;

        }

        public async Task<Transaction> DepositAsync(int accountId, WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto)
        {
            var account = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);
            if (account == null)
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

        public async Task<Transaction> WithdrawlAsync(int accountId, WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto)
        {
            var account = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);
            if (account == null)
            {
                return null;
            }

            if(await IsPrivateAccount(accountId) == false)
            {
                throw new Exception("Withdrawl error. Withdrawl only from private account");
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

        public async Task<bool> IsPrivateAccount(int accountId)
        {
            var account = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);

            if(account != null)
            {
                return account.AccountType == "Private";
            }
            return false; // Account does not exist or not private
        }

        public async Task<bool> UserHasPrivateAccount(int userId)
        {
            return await _bankDbContext.Accounts.AnyAsync(x => x.UserId == userId && x.AccountType == "Private");
        }

    }
}
