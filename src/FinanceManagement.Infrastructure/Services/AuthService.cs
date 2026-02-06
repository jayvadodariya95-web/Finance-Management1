using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore; // NEEDED for .Include()
using FinanceManagement.Infrastructure.Data;
using System.Security.Cryptography;
using FinanceManagement.Application.DTOs; // NEEDED for DbContext

namespace FinanceManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    // CHANGE 1: Use Context directly to handle complex Includes easily
    private readonly FinanceDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(FinanceDbContext context, IUserRepository userRepository, IConfiguration configuration)
    {
        _context = context;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    //public async Task<string> LoginAsync(string email, string password)
    //{
    //    //// BUG: No rate limiting or account lockout
    //    //var user = await _userRepository.GetByEmailAsync(email);
    //    // CHANGE 2: Load User AND their Partner profile
    //    var user = await _context.Users
    //        .Include(u => u.Partner) // <--- CRITICAL: This loads the Partner ID
    //        .FirstOrDefaultAsync(u => u.Email == email);

    //    if (user == null || !user.IsActive)
    //    {
    //        // BUG: Same response time for invalid user and wrong password (timing attack)
    //        return string.Empty;
    //    }

    //    // BUG: No protection against timing attacks
    //    if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
    //    {
    //        return string.Empty;
    //    }

    //    return GenerateJwtToken(user);
    //}

    public async Task<LoginResponseDto?> LoginAsync(string email, string password)
    {
        var user = await _context.Users
            .Include(u => u.Partner)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !user.IsActive)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.ToString()
            }
        };
    }

    //public async Task<string> RefreshTokenAsync(string token)
    //{
    //    try
    //    {
    //        // BUG: No proper refresh token implementation - just validating and regenerating
    //        var isValid = await ValidateTokenAsync(token);
    //        if (!isValid)
    //        {
    //            return string.Empty;
    //        }

    //        // BUG: Extracting user from expired token without proper validation
    //        var handler = new JwtSecurityTokenHandler();
    //        var jsonToken = handler.ReadJwtToken(token);
    //        var emailClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

    //        if (emailClaim == null)
    //        {
    //            return string.Empty;
    //        }

    //        //var user = await _userRepository.GetByEmailAsync(emailClaim.Value);
    //        //if (user == null)
    //        //{
    //        //    return string.Empty;
    //        //}

    //        // Update refresh logic to also include Partner
    //        var user = await _context.Users
    //            .Include(u => u.Partner)
    //            .FirstOrDefaultAsync(u => u.Email == emailClaim.Value);

    //        if (user == null) return string.Empty;

    //        return GenerateJwtToken(user);
    //    }
    //    catch
    //    {
    //        return string.Empty;
    //    }
    //}

    public async Task<LoginResponseDto?> RefreshTokenAsync(string accessToken ,string refreshToken )
    {
        var storedToken = await _context.RefreshTokens
            .Include(r => r.User)
            .ThenInclude(u => u.Partner)
            .FirstOrDefaultAsync(r =>
                r.Token == refreshToken &&
                !r.IsRevoked &&
                r.ExpiresAt > DateTime.UtcNow);

        if (storedToken == null)
            return null;

        // Revoke old token
        storedToken.IsRevoked = true;

        // Create new refresh token
        var newRefreshToken = GenerateRefreshToken();
        _context.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            UserId = storedToken.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        });

        await _context.SaveChangesAsync();

        var newAccessToken = GenerateJwtToken(storedToken.User);

        return new LoginResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            User = new UserDto
            {
                Id = storedToken.User.Id,
                Email = storedToken.User.Email,
                Role = storedToken.User.Role.ToString()
            }
        };
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]!);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out _);

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }


    //private string GenerateJwtToken(User user)
    //{
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "");

    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new[]
    //        {
    //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //            new Claim(ClaimTypes.Email, user.Email),
    //            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
    //            new Claim(ClaimTypes.Role, user.Role.ToString())
    //        }),
    //        Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "60")),
    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
    //        Issuer = _configuration["JwtSettings:Issuer"],       // <-- add this
    //        Audience = _configuration["JwtSettings:Audience"]

    //        // BUG: Not setting Issuer and Audience
    //    };

    //    var token = tokenHandler.CreateToken(tokenDescriptor);
    //    return tokenHandler.WriteToken(token);
    //}

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }


    private string GenerateJwtToken(User user)
    {
        //var tokenHandler = new JwtSecurityTokenHandler();
        //var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]!);

        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET")
            ?? throw new Exception("JWT_SECRET not configured");

        var key = Encoding.UTF8.GetBytes(secretKey);
        var tokenHandler = new JwtSecurityTokenHandler();

        //var tokenDescriptor = new SecurityTokenDescriptor
        //{
        //    Subject = new ClaimsIdentity(new[]
        //    {
        //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //    new Claim(ClaimTypes.Email, user.Email),
        //    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
        //    new Claim(ClaimTypes.Role, user.Role.ToString()) // "Admin"
        //}),

        // Create the basic list of claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        // CHANGE 3: Add PartnerId Claim if it exists
        if (user.Partner != null)
        {
            // This allows the Controller to check User.FindFirst("PartnerId")
            claims.Add(new Claim("PartnerId", user.Partner.Id.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["JwtSettings:ExpiryMinutes"]!)
            ),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"]
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