namespace BankAPI.DataTransferObjects.ResponseDtos
{
    public class AccountResponseDto
    {
        public int AccountId { get; set; }

        public int? UserId { get; set; }

        public string? AccountNumber { get; set; }

        public string AccountType { get; set; } = null!;

        public decimal? Balance { get; set; }
    }
}
