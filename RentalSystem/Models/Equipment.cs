namespace RentalSystem.Models;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } 
    public string Size { get; set; }
    public string Condition { get; set; }
    public decimal RentalPricePerHour { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
}