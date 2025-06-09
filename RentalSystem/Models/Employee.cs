namespace RentalSystem.Models;

public class Employee
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // admin, manager, staff
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
}