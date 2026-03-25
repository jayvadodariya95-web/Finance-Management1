using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Helpers;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationParams paginationParams)
        {
            var users = await _userService.GetAllAsync(paginationParams);

            return Ok(users);
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound($"User with ID {id} not found");

            return Ok(user);
        }

        // GET: api/user/email/test@test.com
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var user = await _userService.GetByEmailAsync(email);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userService.CreateAsync(user);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = createdUser.Id },
                createdUser);
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO user)
        {

            var updatedUser = await _userService.UpdateAsync(user, id); 
            return Ok(updatedUser);
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPatch("/user/{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] PatchUserDTO user)
        {
            var updatedUser = await _userService.PatchAsync(user, id);

            return Ok(updatedUser);
        }


        
    }
    [ApiController]
    [Route("api/[controller]")]
    public class PartnerController : ControllerBase
    {
        private readonly IPartnerServices _partnerServices;
        private readonly IUserServices _userServices;

        public PartnerController(IPartnerServices partnerServices, IUserServices userServices)
        {
            this._partnerServices = partnerServices;
            this._userServices = userServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPartner()
        {
            var result = await _partnerServices.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("/api/partner")]
        public async Task<IActionResult> CreateAsync(PartnerDto partner)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPartner = await _partnerServices.CreateAsync(partner);

            return Ok(createdPartner);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPartner(int id, [FromBody] UpdatedPartnerDTO partner)
        {
            var patchUser = await _partnerServices.PatchAsync(id,partner);
            return Ok(patchUser);
        }

        [HttpGet("/api/getUser-partner/{userid}")]
        public async Task<IActionResult> GetPartnerByUserID([FromRoute(Name = "userid")] int userId)
        {
            var result = await _partnerServices.GetByUserID(userId);
            return Ok(result);
        }

        [HttpGet("/api/partner/{id}")]
        public async Task<IActionResult> GetPartnerById(int id)
        {
            try
            {
                var result = await _partnerServices.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Partner not found",
                        data = (object)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Partner fetched successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message, // for debugging (later hide this)
                    data = (object)null
                });
            }
        }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;

        public EmployeeController(IEmployeeServices empServices)
        {
            _employeeServices = empServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var users = await _employeeServices.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByID(int id)
        {
            var emp = await _employeeServices.GetByIdAsync(id);

            if (emp == null)
                return NotFound("Employee not found");

            return Ok(emp);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEmployeeByUserID(int userId)
        {
            var empUser = await _employeeServices.GetEmployeeByUserIdAsync(userId);

            //if (empUser == null)
            //    return NotFound("Employee not found for this user");

            return Ok(empUser);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeDto employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEmp = await _employeeServices.CreateAsync(employee);
            return Ok(newEmp);
        }
    }
}