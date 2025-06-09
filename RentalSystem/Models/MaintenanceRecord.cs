namespace RentalSystem.Models;

public class MaintenanceRecord
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string Description { get; set; }
    public string PerformedBy { get; set; }
    public decimal? Cost { get; set; }
}