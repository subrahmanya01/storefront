using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Storefront.UserService.Entities;
using Storefront.UserService.Infrastructure.Repository;
using Storefront.UserService.Models;
using Storefront.UserService.Models.Request;
using Storefront.UserService.Models.Response;

namespace Storefront.UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<IResult<UserResponse>> UpdateUser(Guid userId, UserUpdateRequest request)
        {
            var existingUser = await _userRepository.GetAsync(x => x.Id == userId);

            if (existingUser == null)
            {
                return Result<UserResponse>.Fail("User not found", StatusCodes.Status404NotFound);
            }

            if ((!string.IsNullOrWhiteSpace(request.CurrentPassword) || !string.IsNullOrWhiteSpace(request.NewPassword) ) && _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password!, request.CurrentPassword!) != PasswordVerificationResult.Success)
            {
                return Result<UserResponse>.Fail("Invalid password", StatusCodes.Status400BadRequest);
            }

            var mappedUser = _mapper.Map<(User, UserUpdateRequest), User>((existingUser, request));
            var result = await _userRepository.UpdateAsync(mappedUser);
            return Result<UserResponse>.Ok(_mapper.Map<UserResponse>(result));
        }
    }
}
