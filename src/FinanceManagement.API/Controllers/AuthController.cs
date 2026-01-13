using Microsoft.AspNetCore.Mvc;
using FinanceManagement.Application.Common;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Application.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace FinanceManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, IUserRepository userRepository, ILogger<AuthController> logger)
    {
        _authService = authService;
        _userRepository = userRepository;
        _logger = logger;
    }

    //[HttpPost("login")]
    //public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
    //{
    //    try
    //    {
    //        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

    //        var result = await _authService.LoginAsync(request.Email, request.Password);

    //        if (result == null)
    //        {
    //            return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResult("Invalid credentials"));
    //        }

    //        var user = await _userRepository.GetByEmailAsync(request.Email);

    //        if (user == null)
    //        {
    //            _logger.LogWarning("User not found after successful token generation for {Email}", request.Email);
    //            return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResult("Invalid credentials"));
    //        }

    //        var response = new LoginResponseDto
    //        {
    //            Token = token,
    //            User = new UserDto
    //            {
    //                Id = user.Id,
    //                FirstName = user.FirstName,
    //                LastName = user.LastName,
    //                Email = user.Email,
    //                Role = user.Role.ToString(),
    //                IsActive = user.IsActive
    //            }
    //        };

    //        return Ok(ApiResponse<LoginResponseDto>.SuccessResult(response, "Login successful"));
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Login failed for {Email}", request.Email);
    //        return StatusCode(500, ApiResponse<LoginResponseDto>.ErrorResult("Login failed"));
    //    }
    //}

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(
    [FromBody] LoginRequestDto request)
    {
        try
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (result == null)
            {
                return Unauthorized(
                    ApiResponse<LoginResponseDto>.ErrorResult("Invalid credentials"));
            }

            return Ok(
                ApiResponse<LoginResponseDto>.SuccessResult(result, "Login successful"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for {Email}", request.Email);
            return StatusCode(
                500,
                ApiResponse<LoginResponseDto>.ErrorResult("Login failed"));
        }
    }


    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<string>>> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (result == null)
            {
                return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResult("Invalid or expired refresh token"));
            }

            return Ok(ApiResponse<LoginResponseDto>.SuccessResult(result, "Token refreshed successfully"));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token refresh failed");
            return StatusCode(500, ApiResponse<string>.ErrorResult("Token refresh failed"));
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] CreateUserDto request)
    {
        try
        {
            //var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            //if (existingUser != null)
            //{
            //    return BadRequest(ApiResponse<UserDto>.ErrorResult("Email already exists"));
            //}

            var email = request.Email.Trim().ToLower();

            if (!Enum.IsDefined(typeof(FinanceManagement.Domain.Enums.UserRole), request.Role))
            {
                return BadRequest(ApiResponse<UserDto>.ErrorResult("Invalid role"));
            }

            var user = new FinanceManagement.Domain.Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                //PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = (FinanceManagement.Domain.Enums.UserRole)request.Role,
                IsActive = true
            };

            var createdUser = await _userRepository.CreateAsync(user);

            var userDto = new UserDto
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                Role = createdUser.Role.ToString(),
                IsActive = createdUser.IsActive
            };

            return Ok(ApiResponse<UserDto>.SuccessResult(userDto, "User registered successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User registration failed for email {Email}", request.Email);
            return StatusCode(500, ApiResponse<UserDto>.ErrorResult("Registration failed"));
        }

    }
}

//public class LoginRequestDto
//{
//    public string Email { get; set; } = string.Empty;
//    public string Password { get; set; } = string.Empty;
//}

//public class LoginResponseDto
//{
//    public string Token { get; set; } = string.Empty;
//    public UserDto User { get; set; } = new();
//}

//public class RefreshTokenRequestDto
//{
//    public string Token { get; set; } = string.Empty;
//}