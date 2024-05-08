namespace BankAPI.DataTransferObjects.RequestDtos
{
    public class TransferRequestDto
    {
        public int ToAccountId { get; set; }

        public decimal Amount { get; set; }

    }
}
