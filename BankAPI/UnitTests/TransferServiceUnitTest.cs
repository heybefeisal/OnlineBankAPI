using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;
using BankAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankAPI.UnitTests
{
    [TestClass]
    public class TransferServiceUnitTest: UnitTestBase
    {
        [TestMethod]
        public async Task TransferAsyc_ReturnTransaction_WhenTransferIsSuccessful()
        {
            //Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var amount = 100;

            var transferRequestDto = new TransferRequestDto
            {
                ToAccountId = toAccountId,
                Amount = amount
            };

            var fromAccount = new Account { AccountId = fromAccountId, Balance = 500 };
            var toAccount = new Account { AccountId = toAccountId, Balance = 200 };

            var mockDbSet = new Mock<DbSet<Account>>();
            mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) =>
                {
                    var accountId = (int)ids[0];
                    return accountId == fromAccountId ? fromAccount : accountId == toAccountId ? toAccount : null;
                });

            var mockDbContext = new Mock<BankDbContext>();
            mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var transferService = new TransferService(mockDbContext.Object);

            //Act
            var result = await transferService.TransferAsync(fromAccountId, transferRequestDto);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fromAccountId, result.FromAccountId);
            Assert.AreEqual(toAccountId, result.ToAccountId);
            Assert.AreEqual(amount, result.Amount);

            //Check if balance updated
            Assert.AreEqual(400, fromAccount.Balance); //500 - 100
            Assert.AreEqual(300, toAccount.Balance); //200 +  100
        }

        [TestMethod]
        public async Task TransferAsync_ReturnsNull_WhenFromAccountDoesNotExist()
        {
            //Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var amount = 100;

            var transferRequestDto = new TransferRequestDto
            {
                ToAccountId = toAccountId,
                Amount = amount
            };

            var mockDbSet = new Mock<DbSet<Account>>();
            mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Account)null);

            var mockDbContext = new Mock<BankDbContext>();
            mockDbContext.Setup(x => x.Accounts).Returns(mockDbSet.Object);

            var transferService = new TransferService(mockDbContext.Object);

            //Act
            var result = await transferService.TransferAsync(fromAccountId, transferRequestDto);

            //Assert
            Assert.IsNull(result);
        }
    }
}
