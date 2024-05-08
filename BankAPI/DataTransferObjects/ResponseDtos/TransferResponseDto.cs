namespace BankAPI.DataTransferObjects.ResponseDtos
{
    public class TransferResponseDto
    {
        public int TransactionId { get; set; }

        public int? FromAccountId { get; set; }

        public int? ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}
