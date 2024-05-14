using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;
using BankAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAPI.Test.UnitTests
{
        [TestClass]
        public class AccountServiceUnitTest : UnitTestBase
        {
            [TestMethod]
            public async Task GetBalanceAsync_ReturnsBalance_WhenAccountExists() //GetBalanceAsync
            {
                //Arrange
                int userId = 1;
                int accountId = 1;
                decimal balance = 1000;

                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>()))
                    .ReturnsAsync(new Account { UserId = userId, AccountId = accountId, Balance = balance });

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);

                var accountService = new AccountService(_mapper, mockDbContext.Object);

                //Act 
                var result = await accountService.GetBalanceAsync(userId, accountId);

                //Assert
                Assert.AreEqual(balance, result);
            }

            [TestMethod]
            public async Task GetBalance_ReturnsNull_WhenAccountDoesNotExist() //GetBalanceAsync
            {
                //Arrange
                int userId = 1;
                int accountId = 1;

                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[][]>())).ReturnsAsync((Account)null);

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);

                var accountService = new AccountService(_mapper, mockDbContext.Object);

                //Act
                var result = await accountService.GetBalanceAsync(userId, accountId);

                //Assert
                Assert.IsNull(result);
            }

            [TestMethod]
            public async Task CreateAccount_ReturnsAccount_WhenUserExistsAndNoPrivateAccount() //CreateAccountAsync
            {
                //Arrange
                var userId = 1;
                var accountRequestDto = new AccountRequestDto
                {
                    AccountNumber = "123412321",
                    AccountType = "Spar",
                    Balance = 1000
                };

                var user = new Account { UserId = userId };
                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync(user);

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);
                mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

                var userServiceMock = new Mock<AccountService>(MockBehavior.Strict, _mapper, mockDbContext.Object, null);
                userServiceMock.Setup(x => x.UserHasPrivateAccount(userId)).ReturnsAsync(false);

                var accountService = userServiceMock.Object;

                //Act
                var result = await accountService.CreateAccountAsync(userId, accountRequestDto);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(userId, result.UserId);
                Assert.AreEqual(accountRequestDto.AccountNumber, result.AccountNumber);
                Assert.AreEqual(accountRequestDto.AccountType, result.AccountType);
                Assert.AreEqual(accountRequestDto.Balance, result.Balance);
            }

            [TestMethod]
            public async Task CreateAccount_ThrowsException_WhenUserExistsAndPrivateAccountExists() //CreateAccountAsync
            {
                //Arrange
                var userId = 1;
                var accountRequestDto = new AccountRequestDto
                {
                    AccountNumber = "123412321",
                    AccountType = "Private",
                    Balance = 1000
                };

                var user = new Account { UserId = userId };
                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync(user);

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);
                mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

                var userServiceMock = new Mock<AccountService>(MockBehavior.Strict, _mapper, mockDbContext.Object, null);
                userServiceMock.Setup(x => x.UserHasPrivateAccount(userId)).ReturnsAsync(true);

                var accountService = userServiceMock.Object;

                //Act
                Exception caughtException = null;
                try
                {
                    await accountService.CreateAccountAsync(userId, accountRequestDto);
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                //Assert
                Assert.IsNotNull(caughtException);
                Assert.AreEqual("Private account exists", caughtException.Message);
            }

            [TestMethod]
            public async Task CreateAccountAsync_ReturnsNull_WhenUserDoesNotExist() //CreateAccountAsync
            {
                //Arrange
                var userId = 1;
                var accountRequestDto = new AccountRequestDto
                {
                    AccountNumber = "123412321",
                    AccountType = "Spar",
                    Balance = 1000
                };

                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Account)null);

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);

                var accountService = new AccountService(_mapper, mockDbContext.Object);

                //Act
                var result = await accountService.CreateAccountAsync(userId, accountRequestDto);

                //Assert
                Assert.IsNull(result);
            }

            [TestMethod]
            public async Task DepositAsync_ReturnsTransaction_WhenAccountExists() //DepositAsync
            {
                var accountId = 1;
                var amount = 100;
                var withdrawlRequestDto = new WithdrawlOrDepositRequestDto { Amount = amount };

                var account = new Account { AccountId = accountId, Balance = 500 };
                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync(account);

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);
                mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

                var accountService = new AccountService(_mapper, mockDbContext.Object);

                //Act
                var result = await accountService.DepositAsync(accountId, withdrawlRequestDto);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(accountId, result.ToAccountId);
                Assert.AreEqual(accountId, result.FromAccountId);
                Assert.AreEqual(amount, result.Amount);
            }

            [TestMethod]
            public async Task DepositAsync_ReturnsNull_WhenAccountDoesNotExist() //DepositAsync
            {
                var accountId = 1;
                var amount = 100;
                var withdrawlRequestDto = new WithdrawlOrDepositRequestDto { Amount = amount };

                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Account)null);

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);

                var accountService = new AccountService(_mapper, mockDbContext.Object);

                //Act
                var result = await accountService.DepositAsync(accountId, withdrawlRequestDto);

                //Assert
                Assert.IsNull(result);
            }

            [TestMethod]
            public async Task WithdrawlAsync_ReturnsTransaction_WhenAccountExistsAndIsPrivate() //WithdrawlAsync
            {
                //Arrange
                var accountId = 1;
                var amount = 100;
                var withdrawlRequestDto = new WithdrawlOrDepositRequestDto { Amount = amount };

                var account = new Account { AccountId = accountId, Balance = 500, AccountType = "Private" };

                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync(account);

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);
                mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

                var accountService = new AccountService(_mapper, mockDbContext.Object);

                //Act
                var result = await accountService.WithdrawlAsync(accountId, withdrawlRequestDto);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(accountId, result.ToAccountId);
                Assert.AreEqual(accountId, result.FromAccountId);
                Assert.AreEqual(amount, result.Amount);
            }

            [TestMethod]
            public async Task WithdrawlAsync_ReturnsNull_WhenAccountDoesNotExist() //WithdrawlAsync
            {
                //Arrange
                var accountId = 1;
                var amount = 100;
                var withdrawlRequestDto = new WithdrawlOrDepositRequestDto { Amount = amount };

                var account = new Account { AccountId = accountId, Balance = 500 };

                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Account)null);

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);

                var accountService = new AccountService(_mapper, mockDbContext.Object);

                //Act
                var result = await accountService.WithdrawlAsync(accountId, withdrawlRequestDto);

                //Assert
                Assert.IsNull(result);
            }

            [TestMethod]
            public async Task WithdrawlAsync_ThrowsException_WhenAccountIsNotPrivate() //WithdrawlAsync
            {
                //Arrange
                var accountId = 1;
                var amount = 100;
                var withdrawlRequestDto = new WithdrawlOrDepositRequestDto { Amount = amount };

                var account = new Account { AccountId = accountId, Balance = 500, AccountType = "Spar" };

                var mockDbSet = new Mock<DbSet<Account>>();
                mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((account));

                var mockDbContext = new Mock<BankDbContext>();
                mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);

                var accountService = new AccountService(_mapper, mockDbContext.Object);

                //Act
                Exception caughtException = null;
                try
                {
                    await accountService.WithdrawlAsync(accountId, withdrawlRequestDto);
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                //Assert
                Assert.IsNotNull(caughtException);
                Assert.AreEqual("Withdrawal only from private account",caughtException.Message);
            }
        }
    }
