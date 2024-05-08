using BankAPI.Models;

namespace BankAPI.DataTransferObjects.RequestDtos
{
    public class AccountRequestDto
    {
        public string? AccountNumber { get; set; }

        public string AccountType { get; set; } = null!;

        public decimal? Balance { get; set; }

    }
}
