using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storefront.UserService.Entities;
using Storefront.UserService.Infrastructure.Repository;
using Storefront.UserService.Models.Request;
using Storefront.UserService.Models.Response;
using Storefront.UserService.Services;
using System.Text.RegularExpressions;

namespace Storefront.UserService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserController(ILogger<UserController> logger, IUserRepository userRepository, IMapper mapper, IHttpContextAccessor contextAccessor, IUserService userService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _userService = userService;
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult> GetUserById(Guid id) {
            var user = await _userRepository.GetAsync( x=> x.Id.Equals(id));
            if (user == null) { 
                return NotFound($"User with id {id} not found");
            }
            return Ok(_mapper.Map<UserResponse>(user));
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<ActionResult> GetUser()
        {
            var userInfo = _contextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }
            var userId = Guid.Parse(userIdClaim);
            var user = await _userRepository.GetAsync(x => x.Id.Equals(userId));
            if (user == null)
            {
                return NotFound($"User with id {userId} not found");
            }
            return Ok(_mapper.Map<UserResponse>(user));
        }

        [HttpGet]
        [Authorize]
        [Route("GetByEmail/{email}")]
        public async Task<ActionResult> GetUserByEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email,"^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$"))
            {
                return BadRequest("Please provide valid email");
            }
            var user = await _userRepository.GetAsync(x => EF.Functions.ILike(x.Email!, email));
            if (user == null)
            {
                return NotFound($"User with email {email} not found");
            }
            return Ok(_mapper.Map<UserResponse>(user));
        }

        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> Update(UserUpdateRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userInfo = _contextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if(string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }
            var userId = Guid.Parse(userIdClaim);
            var result = await _userService.UpdateUser(userId, user);
            if(!result.IsSuccess)
            {
                return StatusCode(result.Status, result.Message);
            }
            return Ok(result.Data);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if(result)
            {
                _logger.LogInformation("User with id {userid} deleted successfully", id);
                return StatusCode(StatusCodes.Status200OK, "User deleted successfully");
            }
            _logger.LogError("Faield to delete user with id {userid}", id);
            return StatusCode(StatusCodes.Status304NotModified, "Failed to delete the user");
        }

    }
}
