using AutoMapper;
using BankAPI.Controllers;
using BankAPI.Models;
using BankAPI.Services;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankAPI.UnitTests
{
    [TestClass]
    public class AccountServiceUnitTest : UnitTestBase
    {
        [TestMethod]
        public async Task GetBalanceAsync_ReturnsBalance_WhenAccountExists()
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

            var accountService = new AccountService(_mapper, mockDbContext.Object, null);

            //Act 
            var result = await accountService.GetBalanceAsync(userId, accountId);

            //Assert
            Assert.AreEqual(balance, result);
        }

        [TestMethod]
        public async Task GetBalance_ReturnsNull_WhenAccountDoesNotExist()
        {
            //Arrange
            int userId = 1;
            int accountId = 1;

            var mockDbSet = new Mock<DbSet<Account>>();

            mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[][]>())).ReturnsAsync((Account)null);

            var mockDbContext = new Mock<BankDbContext>();
            mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);

            var accountService = new AccountService(_mapper, mockDbContext.Object, null);

            //Act
            var result = await accountService.GetBalanceAsync(userId, accountId);

            //Assert
            Assert.IsNull(result);
        }
    }
}
