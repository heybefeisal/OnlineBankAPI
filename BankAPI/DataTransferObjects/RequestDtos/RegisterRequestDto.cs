namespace BankAPI.DataTransferObjects.RequestDtos
{
    public class RegisterRequestDto
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
