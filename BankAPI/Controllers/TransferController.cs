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
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;
        private readonly IMapper _mapper;

        public TransferController(ITransferService transferService, IMapper mapper)
        {
            _transferService = transferService;
            _mapper = mapper;
        }

        [HttpPost("{fromAccountId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(TransferResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Transfer([FromRoute] int fromAccountId, [FromBody] TransferRequestDto transactionRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transfer = await _transferService.TransferAsync(fromAccountId, transactionRequestDto);

            if (transfer == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<TransferResponseDto>(transfer);
            return Ok(mappedResult);
        }
    }
}
