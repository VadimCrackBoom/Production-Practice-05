// DTOs/MaintenanceRecordDto.cs
namespace RentalSystem.DTOs
{
    public class MaintenanceRecordDto
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string Description { get; set; }
        public string PerformedBy { get; set; }
        public decimal? Cost { get; set; }
        public string? NewCondition { get; set; } // Опционально: новое состояние оборудования после обслуживания
        public bool? MakeAvailable { get; set; } // Опционально: сделать оборудование доступным после обслуживания
    }

    public class MaintenanceRecordCreateDto
    {
        public int EquipmentId { get; set; }
        public string Description { get; set; }
        public string PerformedBy { get; set; }
        public decimal? Cost { get; set; }
        public string? NewCondition { get; set; }
        public bool? MakeAvailable { get; set; }
    }
}