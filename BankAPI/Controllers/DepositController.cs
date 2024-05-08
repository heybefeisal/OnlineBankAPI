using AutoMapper;
using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.DataTransferObjects.ResponseDtos;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly IAccountService _accountservice;
        private readonly IMapper _mapper;

        public DepositController(IAccountService accountService, IMapper mapper)
        {
            _accountservice = accountService;
            _mapper = mapper;
        }

        [HttpPost("{accountId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(WithdrawlOrDepositResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DepositMoney([FromRoute] int accountId, [FromBody] WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deposit = await _accountservice.DepositAsync(accountId, withdrawlOrDepositRequestDto);

            if (deposit == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<WithdrawlOrDepositResponseDto>(deposit);
            return Ok(mappedResult);
        }
    }
}
