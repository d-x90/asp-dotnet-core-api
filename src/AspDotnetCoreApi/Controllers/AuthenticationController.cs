using System.Threading.Tasks;
using AutoMapper;
using AspDotnetCoreApi.Dtos;
using AspDotnetCoreApi.Exceptions;
using AspDotnetCoreApi.Models;
using AspDotnetCoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using AspDotnetCoreApi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace AspDotnetCoreApi.Controllers {

    [Route("api/v1/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase {
        private readonly IAuthenticationService _authService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IAuthenticationService authService, IMapper mapper, IUserRepository userRepository)
        {
            _authService = authService;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] AuthRegisterRequestDto registerDto) {
            var user = _mapper.Map<User>(registerDto);
            
            if(!registerDto.Password.Equals(registerDto.PasswordConfirm)) {
                return BadRequest(new {
                    message = "Password mismatch"
                });
            }

            if(await _authService.IsUserPresent(user.Username, user.Email)) {
                return BadRequest("User already exists with this username or email");
            }

            var userId = await _authService.Register(user, registerDto.Password);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtAuthResponseDto>> Login([FromBody] AuthLoginResquestDto loginDto) {
            loginDto.usernameOrEmail = loginDto.usernameOrEmail.Trim().ToLower();
            loginDto.password = loginDto.password.Trim();

            var responseDto = new JwtAuthResponseDto();

            try {
                responseDto.JwtToken = await _authService.Login(loginDto.usernameOrEmail, loginDto.password);
            } catch (UserNotFoundException) {
                return BadRequest(new {
                    message = "User not found"
                });
            } catch (IncorrectPasswordException) {
                return BadRequest(new {
                    message = "Incorrect password"
                });
            }

            return Ok(responseDto);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult> GetCurrentUser() {
            return Ok(await _userRepository.GetCurrentUserAsync());
        }
    }
}