namespace RentalSystem.Models;

public class Rental
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime RentalStart { get; set; }
    public DateTime? RentalEnd { get; set; }
    public decimal TotalCost { get; set; }
    public bool IsPaid { get; set; }
    public string Status { get; set; }
    public List<RentalEquipment> RentalEquipments { get; set; } = new List<RentalEquipment>();
}