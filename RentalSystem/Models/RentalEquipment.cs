namespace RentalSystem.Models;

public class RentalEquipment
{
    public int RentalId { get; set; }
    public Rental Rental { get; set; }
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPriceAtRental { get; set; }
}