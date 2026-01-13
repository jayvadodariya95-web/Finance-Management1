using FinanceManagement.Application.Common;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();

            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            });

            return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResult(userDtos));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserDto>>> Create([FromBody] CreateUserDto request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Role = request.Role 
                
            };

            var users = await _userRepository.CreateAsync(user);

            var userDto = new UserDto
            {
                Id = users.Id,
                FirstName = users.FirstName,
                LastName = users.LastName,
                Email = users.Email,
                Role = users.Role.ToString()

            };

            return Ok(ApiResponse<UserDto>.SuccessResult(userDto, "User created Successfully"));
        }

    }
}
