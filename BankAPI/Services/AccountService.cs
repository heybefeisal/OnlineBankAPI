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

        public async Task<Account> CreateAccountAsync(int userId, AccountRequestDto accountRequestDto)
        {
            var user = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.UserId == userId);

            if(user == null) 
            {
                return null;
            }

            //var accountExists = AccountExistsAsync(accountRequestDto.AccountNumber);

            //if(accountExists == null)
            //{
            //    throw new Exception("Account Exists");
            //}

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

        //public async Task<Account> AccountExistsAsync(string accountNumber)
        //{
        //    var account = await _bankDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

        //    if (account == null)
        //    {
        //        return null;
        //    }

        //    return account;
        //}
    }
}
