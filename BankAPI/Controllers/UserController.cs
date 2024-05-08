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
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.CreateUserAsync(userRequestDto);

            if (user == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<UserResponseDto>(user);
            return Ok(mappedResult);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserRequestDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById([FromRoute] int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var mappedResult = _mapper.Map<UserResponseDto>(user);
            return Ok(mappedResult);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            if(!users.Any())
            {
                return NoContent();
            }

            var mappedResult = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return Ok(mappedResult);
        }


        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserRequestDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] UserRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.UpdateUserAsync(userId, userRequestDto);

            if (user == null)
            {
                return BadRequest();
            }

            var mappedResult = _mapper.Map<UserRequestDto>(user);
            return Ok(mappedResult);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId)
        {
            var user = await _userService.DeleteUser(userId);

            if (!user)
            {
                return BadRequest();
            }

            return Ok(userId);
        }


    }
}
