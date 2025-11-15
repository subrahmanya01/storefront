using Microsoft.AspNetCore.Mvc;
using Storefront.UserService.Models.Request;
using Storefront.UserService.Services;

namespace Storefront.UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService; 
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authService.HandleLogin(loginRequest);
            if (!response.Success) {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.HandleRegister(registerRequest);
            if (!response.Success)
            {
                return Conflict(response);
            }
            _logger.LogInformation("New user created successfully");
            return Created("", response);
        }

        [HttpPost]
        [Route("CompleteRegistration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] CompleteRegistrationRequest model)
        {

            if (string.IsNullOrWhiteSpace(model.UniqueId?.ToString()))
            {
                return BadRequest("Invalid Request");
            }

            var response = await _authService.CompleteRegistration(model);
            if (!response.Success)
            {
                return Conflict(response);
            }
            _logger.LogInformation("New user created successfully");
            return Created("", response);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            if(string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest("Email required");
            }

            var response = await _authService.HandleForgotPassword(model);
            return Ok("Email sent successfully, please check your mail box to reset your password");
        }

        [HttpPost]
        [Route("ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.NewPassword) && model.NewPassword.Length > 8)
            {
                return BadRequest("Password is required and it should have length greater than or equal to 8.");
            }

            var response = await _authService.ResetPassword(id, model);
            return Ok("Email sent successfully, please check your mail box to reset your password");
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.HandleRefreshToken(refreshTokenRequest);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
    }
}
