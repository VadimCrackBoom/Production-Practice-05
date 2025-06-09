using Microsoft.AspNetCore.Mvc;

namespace RentalSystem.DTOs;

public class EmployeeDto : IActionResult
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime HireDate { get; set; }
    
    public Task ExecuteResultAsync(ActionContext context)
    {
        throw new NotImplementedException();
    }
}

public class EmployeeLoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class EmployeeRegisterDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
}