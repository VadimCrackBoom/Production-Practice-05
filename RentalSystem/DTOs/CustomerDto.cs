using Microsoft.AspNetCore.Mvc;

namespace RentalSystem.DTOs;

public class CustomerDto : IActionResult
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? DocumentNumber { get; set; }
    public DateTime RegistrationDate { get; set; }
    
    public Task ExecuteResultAsync(ActionContext context)
    {
        throw new NotImplementedException();
    }
}

public class CustomerCreateDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? DocumentNumber { get; set; }
}