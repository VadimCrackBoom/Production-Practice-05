using Microsoft.AspNetCore.Mvc;

namespace RentalSystem.DTOs;

public class EquipmentDto : IActionResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Size { get; set; }
    public decimal RentalPricePerHour { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    
    public Task ExecuteResultAsync(ActionContext context)
    {
        throw new NotImplementedException();
    }
}

public class EquipmentCreateDto
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Size { get; set; }
    public decimal RentalPricePerHour { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
}