namespace RentalSystem.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? DocumentNumber { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}