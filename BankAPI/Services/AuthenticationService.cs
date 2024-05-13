using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly BankDbContext _bankDbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationService(BankDbContext bankDbContext, IConfiguration configuration)
        {
            _bankDbContext = bankDbContext;
            _configuration = configuration;
        }
        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _bankDbContext.Users.SingleOrDefaultAsync(x => x.Username ==  username && x.Password == password);

            if(user == null) 
            {
                return null;
            }

            //auth successful, generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> Register(RegisterRequestDto registerRequestDto)
        {
            var userExists = await _bankDbContext.Users.AnyAsync(x => x.Username == registerRequestDto.Username);

            if(userExists) 
            {
                return null;
            }

            var register = new User
            {
                Username = registerRequestDto.Username,
                Password = registerRequestDto.Password,
                Email = registerRequestDto.Email,
                FullName = registerRequestDto.FullName,
                Address = registerRequestDto.Address,
                PhoneNumber = registerRequestDto.PhoneNumber
            };

            await _bankDbContext.Users.AddAsync(register);
            await _bankDbContext.SaveChangesAsync();
            return register;
        }
    }
}
