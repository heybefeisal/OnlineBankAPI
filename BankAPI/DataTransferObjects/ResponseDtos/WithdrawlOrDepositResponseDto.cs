namespace BankAPI.DataTransferObjects.ResponseDtos
{
    public class WithdrawlOrDepositResponseDto
    {
        public int TransactionId { get; set; }

        public int? ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public DateTime? TransactionDate { get; set; }

    }
}
