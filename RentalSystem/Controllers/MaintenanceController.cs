// Controllers/MaintenanceController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalSystem.Data;
using RentalSystem.DTOs;
using RentalSystem.Models;

namespace RentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : BaseController
    {
        public MaintenanceController(RentalDbContext context, ILogger<MaintenanceController> logger) 
            : base(context, logger)
        {
        }

        // GET: api/Maintenance/Equipment/5
        [HttpGet("Equipment/{equipmentId}")]
        public async Task<IActionResult> GetEquipmentMaintenanceHistory(int equipmentId)
        {
            return await HandleAsync(async () =>
            {
                var maintenanceRecords = await _context.MaintenanceRecords
                    .Where(m => m.EquipmentId == equipmentId)
                    .OrderByDescending(m => m.MaintenanceDate)
                    .Select(m => new
                    {
                        m.Id,
                        m.MaintenanceDate,
                        m.Description,
                        m.PerformedBy,
                        m.Cost
                    })
                    .ToListAsync();

                return maintenanceRecords;
            }, $"Retrieved maintenance history for equipment ID: {equipmentId}");
        }

        // POST: api/Maintenance
        [HttpPost]
        public async Task<IActionResult> AddMaintenanceRecord([FromBody] MaintenanceRecordDto maintenanceDto)
        {
            return await HandleAsync(async () =>
            {
                var equipment = await _context.Equipment.FindAsync(maintenanceDto.EquipmentId);
                if (equipment == null)
                {
                    return NotFound("Equipment not found");
                }

                var record = new MaintenanceRecord
                {
                    EquipmentId = maintenanceDto.EquipmentId,
                    MaintenanceDate = DateTime.UtcNow,
                    Description = maintenanceDto.Description,
                    PerformedBy = maintenanceDto.PerformedBy,
                    Cost = maintenanceDto.Cost
                };

                // Обновляем состояние оборудования
                equipment.Condition = maintenanceDto.NewCondition ?? equipment.Condition;
                equipment.IsAvailable = maintenanceDto.MakeAvailable ?? equipment.IsAvailable;

                _context.MaintenanceRecords.Add(record);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEquipmentMaintenanceHistory), 
                    new { equipmentId = maintenanceDto.EquipmentId }, record);
            }, $"Added maintenance record for equipment ID: {maintenanceDto.EquipmentId}");
        }
    }
}