using AutoMapper;
using BankAPI.DataTransferObjects.Configurations;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.UnitTests
{
    public class UnitTestBase
    {
        public readonly BankDbContext _bankDbContext;
        public readonly IMapper _mapper;

        public UnitTestBase()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseSqlServer(dbName)
                .Options;

            _bankDbContext = new BankDbContext(options);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapConfiguration());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
    }
}
