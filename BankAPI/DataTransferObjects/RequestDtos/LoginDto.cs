﻿namespace BankAPI.DataTransferObjects.RequestDtos
{
    public class LoginDto
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
