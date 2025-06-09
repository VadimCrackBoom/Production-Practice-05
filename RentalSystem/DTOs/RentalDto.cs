using Microsoft.AspNetCore.Mvc;

namespace RentalSystem.DTOs;

public class RentalDto : IActionResult
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime RentalStart { get; set; }
    public DateTime? RentalEnd { get; set; }
    public decimal TotalCost { get; set; }
    public bool IsPaid { get; set; }
    public string Status { get; set; }
    public List<RentalEquipmentDto> Equipments { get; set; } = new List<RentalEquipmentDto>();
    
    public Task ExecuteResultAsync(ActionContext context)
    {
        throw new NotImplementedException();
    }
}

public class RentalCreateDto
{
    public int CustomerId { get; set; }
    public List<RentalEquipmentCreateDto> Equipments { get; set; } = new List<RentalEquipmentCreateDto>();
}

public class RentalEquipmentDto
{
    public int EquipmentId { get; set; }
    public string EquipmentName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPriceAtRental { get; set; }
}

public class RentalEquipmentCreateDto
{
    public int EquipmentId { get; set; }
    public int Quantity { get; set; }
}