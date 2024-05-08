using AutoMapper;
using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.Models;
using BankAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly BankDbContext _bankDbContext;

        public UserService(IMapper mapper, BankDbContext bankDbContext)
        {
            _mapper = mapper;
            _bankDbContext = bankDbContext;

        }
        public async Task<User> CreateUserAsync(UserRequestDto userRequestDto)
        {
            var user = await _bankDbContext.Users.FirstOrDefaultAsync(x => x.Username == userRequestDto.Username);
            if (user == null)
            {
                return null;
            }

            var newUser = new User
            {
                Username = userRequestDto.Username,
                Password = userRequestDto.Password,
                Email = userRequestDto.Email,
                FullName = userRequestDto.FullName,
                Address = userRequestDto.Address,
                PhoneNumber = userRequestDto.PhoneNumber,
            };

            await _bankDbContext.Users.AddAsync(newUser);
            await _bankDbContext.SaveChangesAsync();
            return newUser;

        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _bankDbContext.Users.AsNoTracking().ToListAsync();
            return users;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _bankDbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
            {
                return false;
            }

            _bankDbContext.Users.Remove(user);

            if (await _bankDbContext.SaveChangesAsync() == 1)
            {
                return true;
            }

            return false;

        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _bankDbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            return user;
        }

        public async Task<User> UpdateUserAsync(int userId, UserRequestDto userRequestDto)
        {
            var user = await _bankDbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            user.Username = userRequestDto.Username;
            user.Password = userRequestDto.Password;
            user.Email = userRequestDto.Email;
            user.FullName = userRequestDto.FullName;
            user.Address = userRequestDto.Address;
            user.PhoneNumber = userRequestDto.PhoneNumber;

            await _bankDbContext.SaveChangesAsync();
            return user;
        }

    }
}
