using AutoMapper;
using BankAPI.DataTransferObjects.RequestDtos;
using BankAPI.DataTransferObjects.ResponseDtos;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawlController : ControllerBase
    {
        private readonly IAccountService _accountservice;
        private readonly IMapper _mapper;

        public WithdrawlController(IAccountService accountService, IMapper mapper)
        {
            _accountservice = accountService;
            _mapper = mapper;
        }

        [HttpPut("{accountId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(WithdrawlOrDepositResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Withdrawl([FromRoute] int accountId, [FromBody] WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var withdrawl = await _accountservice.WithdrawlAsync(accountId, withdrawlOrDepositRequestDto);

            if (withdrawl == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<WithdrawlOrDepositResponseDto>(withdrawl);
            return Ok(mappedResult);
        }
    }
}
