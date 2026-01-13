using System.ComponentModel.DataAnnotations;

namespace FinanceManagement.Application.DTOs;

// 1. Request to Login
public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

// 2. Response after Login (The one you were looking for!)
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;          // Access Token (Short Lived)
    public string RefreshToken { get; set; } = string.Empty;   // Refresh Token (Long Lived)
    public UserDto User { get; set; } = new();                 // User Details
}

// 3. Request to Refresh a Token
public class RefreshTokenRequestDto
{
    [Required]
    public string Token { get; set; } = string.Empty;          // The Expired Access Token

    [Required]
    public string RefreshToken { get; set; } = string.Empty;   // The Valid Refresh Token
}