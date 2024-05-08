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
    public class WithdrawlOrDepositController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public WithdrawlOrDepositController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost("{accountId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(WithdrawlOrDepositResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DepositMoney([FromRoute] int accountId, [FromBody] WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var deposit = await _transactionService.DepositMoneyAsync(accountId, withdrawlOrDepositRequestDto);

            if(deposit == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<WithdrawlOrDepositResponseDto>(deposit);
            return Ok(mappedResult);
        }

        [HttpPut("{accountId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(WithdrawlOrDepositResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> WithdrawlMoney([FromRoute] int accountId, [FromBody] WithdrawlOrDepositRequestDto withdrawlOrDepositRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deposit = await _transactionService.WithdrawlMoneyAsync(accountId, withdrawlOrDepositRequestDto);

            if (deposit == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<WithdrawlOrDepositResponseDto>(deposit);
            return Ok(mappedResult);
        }
    }
}
