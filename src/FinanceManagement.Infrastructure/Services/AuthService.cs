using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;

namespace FinanceManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        // BUG: No rate limiting or account lockout
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null || !user.IsActive)
        {
            // BUG: Same response time for invalid user and wrong password (timing attack)
            return string.Empty;
        }

        // BUG: No protection against timing attacks
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return string.Empty;
        }

        return GenerateJwtToken(user);
    }

    public async Task<string> RefreshTokenAsync(string token)
    {
        try
        {
            // BUG: No proper refresh token implementation - just validating and regenerating
            var isValid = await ValidateTokenAsync(token);
            if (!isValid)
            {
                return string.Empty;
            }

            // BUG: Extracting user from expired token without proper validation
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var emailClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            
            if (emailClaim == null)
            {
                return string.Empty;
            }

            var user = await _userRepository.GetByEmailAsync(emailClaim.Value);
            if (user == null)
            {
                return string.Empty;
            }

            return GenerateJwtToken(user);
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "");
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // BUG: Should validate issuer
                ValidateAudience = false, // BUG: Should validate audience
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "");
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "60")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            // BUG: Not setting Issuer and Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

// SECURITY VULNERABILITIES:
// 1. No rate limiting for login attempts
// 2. Vulnerable to timing attacks
// 3. No proper refresh token implementation
// 4. Missing issuer and audience validation
// 5. No account lockout mechanism
// 6. Weak token validation