using AutoMapper;
using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.DataTransferObjects.ResponseDtos;
using BankAPI.Models;

namespace BankAPI.DataTransferObjects.Configurations
{
    public class MapConfiguration: Profile
    {
        public MapConfiguration()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<UserResponseDto, User>();

            CreateMap<UserRequestDto, User>();
            CreateMap<User, UserRequestDto>();

            CreateMap<Account, AccountResponseDto>();
            CreateMap<AccountResponseDto, Account>();

            CreateMap<AccountRequestDto, Account>();
            CreateMap<Account, AccountRequestDto>();

            CreateMap<Transaction, WithdrawlOrDepositResponseDto>();
            CreateMap<WithdrawlOrDepositResponseDto, Transaction>();

            CreateMap<WithdrawlOrDepositRequestDto, Transaction>();
            CreateMap<WithdrawlOrDepositRequestDto, Transaction>();

            CreateMap<Transaction, TransferResponseDto>();
            CreateMap<TransferResponseDto, Transaction>();

            CreateMap<TransferRequestDto, Transaction>();
            CreateMap<TransferRequestDto, Transaction>();

        }       
    }
}
