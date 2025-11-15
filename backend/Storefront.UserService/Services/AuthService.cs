using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Storefront.UserService.Entities;
using Storefront.UserService.Infrastructure.Repository;
using Storefront.UserService.Models.Request;
using Storefront.UserService.Models.Response;

namespace Storefront.UserService.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMemoryCache _cache;
        private readonly INotificationService _notificationService;

        private readonly PasswordHasher<User> _passwordHasher;
        public AuthService(ILogger<AuthService> logger, IMapper mapper, IUserRepository userRepository, INotificationService notificationService, IJwtProvider jwtProvider, IMemoryCache cache)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = new PasswordHasher<User>();
            _cache = cache;
            _notificationService = notificationService;
        }

        public async Task<AuthResponse> HandleLogin(LoginRequest request)
        {
            var existingUser = await _userRepository.GetAsync(x =>
                                    (!string.IsNullOrEmpty(x.Email) && x.Email == request.Email.ToLower()) ||
                                    (!string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber == request.PhoneNumber.ToLower())
                                );
            if (existingUser == null) {
                return new AuthResponse(false, "Invalid user or password");
            }

            if (_passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password!, request.Password) != PasswordVerificationResult.Success)
            {
                return new AuthResponse(false, "Invalid password");
            }
            var token = _jwtProvider.Genarate(existingUser, out DateTime expiresAt);
            var refreshToken = await GenarateAndSaveRefreshToken(existingUser);
            return new AuthResponse(true, "Login successful", token, expiresAt, refreshToken);
        }

        public async Task<AuthResponse> HandleRegister(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetAsync(x =>
                                    (!string.IsNullOrEmpty(x.Email) && x.Email == request.Email.ToLower()) ||
                                    (!string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber == request.PhoneNumber.ToLower())
                                );
            if(existingUser != null)
            {
                return new AuthResponse(false, "Email or phone number already in use");
            }
            var user = _mapper.Map<User>(request);
            var hashedPassword = _passwordHasher.HashPassword(user, user.Password!);

            user.Password = hashedPassword;

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
            };

            var uniqueIdentifier = Guid.NewGuid().ToString();

            _cache.Set(uniqueIdentifier, JsonConvert.SerializeObject(user), cacheEntryOptions);
            await _notificationService.SendVerifyEmailMail(user.Email!, uniqueIdentifier);
            return new AuthResponse(true, "Registration email sent sucessfully, Verify your email and complete the registration");
        }

        public async Task<AuthResponse> CompleteRegistration(CompleteRegistrationRequest request)
        {
            _cache.TryGetValue(request.UniqueId.ToString()!, out string? userString);

            if(string.IsNullOrEmpty(userString))
            {
                return new AuthResponse(false, "Request expired or invalid request");
            }
            var user = JsonConvert.DeserializeObject<User>(userString);
            var newlyCreatedUser = await _userRepository.AddAsync(user);

            var token = _jwtProvider.Genarate(newlyCreatedUser, out DateTime expiresAt);
            var refreshToken = await GenarateAndSaveRefreshToken(newlyCreatedUser);
            return new AuthResponse(true, "User registered successfully", token, expiresAt, refreshToken);
        }

        public async Task<AuthResponse> HandleRefreshToken(RefreshTokenRequest request)
        {
            var user = await ValidateRefreshToken(request.UserId, request.RefreshToken!);
            if(user == null)
            {
                return new AuthResponse(false, "Invalid user or refresh token");
            }
            var token = _jwtProvider.Genarate(user, out DateTime expiresAt);
            return new AuthResponse(true, "Token refreshed successfully", token, expiresAt, request.RefreshToken);
        }

        private async Task<User?> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            var user = await _userRepository.GetAsync(x => x.Id == userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        private async Task<string> GenarateAndSaveRefreshToken(User user)
        {
            var refreshToken = _jwtProvider.GenarateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(_jwtProvider.JwtRefreshExpirationInHours);
            await _userRepository.UpdateAsync(user);
            return refreshToken;
        }

        public async Task<bool> HandleForgotPassword(ForgotPasswordRequest request)
        {
            var guid = Guid.NewGuid().ToString();
            var uniqueIdentifier = $"{guid}_{DateTime.UtcNow.AddMinutes(30)}";
            var user = await _userRepository.GetAsync(x=> x.Email == request.Email.ToLower());
            if(user != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                };

                _cache.Set(guid, user.Id.ToString());
                await _notificationService.SendResetPasswordEmail(user.Email!, uniqueIdentifier);
                return true;
            }
            return false;
        }

        public async Task<bool> ResetPassword(Guid id, ResetPasswordRequest request)
        {
            var userInfo = _cache.Get(id.ToString());
            if (userInfo != null) {
                var userId = Guid.Parse(userInfo.ToString()!);
                var existingUser = await _userRepository.GetAsync(x=> x.Id == userId);
                existingUser!.Password = _passwordHasher.HashPassword(existingUser, request.NewPassword);
                await _userRepository.UpdateAsync(existingUser);
                _cache.Remove(id.ToString());
                return true;
            }
            return false;
        }
    }
}
