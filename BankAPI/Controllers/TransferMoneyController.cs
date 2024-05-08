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
    public class TransferMoneyController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransferMoneyController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost("{fromAccountId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(TransferResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> TransferMoney([FromRoute] int fromAccountId, [FromBody] TransferRequestDto transactionRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transfer = await _transactionService.TransferMoneyAsync(fromAccountId, transactionRequestDto);

            if (transfer == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<TransferResponseDto>(transfer);
            return Ok(mappedResult);
        }
    }
}
