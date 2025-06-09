// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalSystem.Data;
using RentalSystem.DTOs;
using RentalSystem.Models;

namespace RentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public AuthController(RentalDbContext context, ILogger<AuthController> logger) 
            : base(context, logger)
        {
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] EmployeeLoginDto loginDto)
        {
            return await HandleAsync(async () =>
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Username == loginDto.Username);

                if (employee == null || loginDto.Password != employee.Password)
                {
                    return Unauthorized();
                }

                
                return new EmployeeDto
                {
                    Id = employee.Id,
                    Username = employee.Username,
                    Role = employee.Role,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    PhoneNumber = employee.PhoneNumber,
                    HireDate = employee.HireDate
                };
            }, $"User {loginDto.Username} logged in");
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] EmployeeRegisterDto registerDto)
        {
            return await HandleAsync(async () =>
            {
                // Проверяем, существует ли пользователь с таким именем
                if (await _context.Employees.AnyAsync(e => e.Username == registerDto.Username))
                {
                    return BadRequest("Username already exists");
                }

                var employee = new Employee
                {
                    Username = registerDto.Username,
                    Password = registerDto.Password,
                    Role = registerDto.Role,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber,
                    HireDate = DateTime.UtcNow
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Login), new EmployeeDto
                {
                    Id = employee.Id,
                    Username = employee.Username,
                    Role = employee.Role,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    PhoneNumber = employee.PhoneNumber,
                    HireDate = employee.HireDate
                });
            }, $"Registered new user: {registerDto.Username}");
        }
    }
}