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
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AccountController(IMapper mapper, IAccountService accountService)
        {
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpPost("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(AccountResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAccountAsync([FromBody] AccountRequestDto accountRequestDto, [FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _accountService.CreateAccountAsync(userId, accountRequestDto);

            if (account == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<AccountResponseDto>(account);
            return Ok(mappedResult);
        }

        [HttpGet("Get Balance/{userId}/{accountId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBalance([FromRoute] int userId, [FromRoute] int accountId)
        {
            var balance = await _accountService.GetBalanceAsync(userId, accountId);

            if(balance == null)
            {
                return NotFound();
            }

            return Ok(balance);
        }
    }
}
