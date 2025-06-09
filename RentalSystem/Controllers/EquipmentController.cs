using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalSystem.Data;
using RentalSystem.DTOs;
using RentalSystem.Models;

namespace RentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : BaseController
    {
        public EquipmentController(RentalDbContext context, ILogger<EquipmentController> logger) 
            : base(context, logger)
        {
        }

        // GET: api/Equipment
        [HttpGet]
        public async Task<IActionResult> GetAllEquipment()
        {
            return await HandleAsync(async () =>
            {
                var equipment = await _context.Equipment
                    .Where(e => e.IsAvailable)
                    .Select(e => new EquipmentDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Type = e.Type,
                        Size = e.Size,
                        RentalPricePerHour = e.RentalPricePerHour,
                        IsAvailable = e.IsAvailable,
                        ImageUrl = e.ImageUrl,
                        Description = e.Description
                    })
                    .ToListAsync();

                return equipment;
            }, "Retrieved all available equipment");
        }

        // GET: api/Equipment/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipment(int id)
        {
            return await HandleAsync(async () =>
            {
                var equipment = await _context.Equipment.FindAsync(id);

                if (equipment == null)
                {
                    return NotFound();
                }

                return new EquipmentDto
                {
                    Id = equipment.Id,
                    Name = equipment.Name,
                    Type = equipment.Type,
                    Size = equipment.Size,
                    RentalPricePerHour = equipment.RentalPricePerHour,
                    IsAvailable = equipment.IsAvailable,
                    ImageUrl = equipment.ImageUrl,
                    Description = equipment.Description
                };
            }, $"Retrieved equipment with ID: {id}");
        }

        // POST: api/Equipment
        [HttpPost]
        public async Task<IActionResult> CreateEquipment([FromBody] EquipmentCreateDto equipmentDto)
        {
            return await HandleAsync(async () =>
            {
                var equipment = new Equipment
                {
                    Name = equipmentDto.Name,
                    Type = equipmentDto.Type,
                    Size = equipmentDto.Size,
                    RentalPricePerHour = equipmentDto.RentalPricePerHour,
                    IsAvailable = true,
                    ImageUrl = equipmentDto.ImageUrl,
                    Description = equipmentDto.Description,
                    Condition = "Новое"
                };

                _context.Equipment.Add(equipment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEquipment), new { id = equipment.Id }, equipment);
            }, "Created new equipment");
        }

        // PUT: api/Equipment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipment(int id, [FromBody] EquipmentDto equipmentDto)
        {
            return await HandleAsync(async () =>
            {
                if (id != equipmentDto.Id)
                {
                    return BadRequest();
                }

                var equipment = await _context.Equipment.FindAsync(id);
                if (equipment == null)
                {
                    return NotFound();
                }

                equipment.Name = equipmentDto.Name;
                equipment.Type = equipmentDto.Type;
                equipment.Size = equipmentDto.Size;
                equipment.RentalPricePerHour = equipmentDto.RentalPricePerHour;
                equipment.IsAvailable = equipmentDto.IsAvailable;
                equipment.ImageUrl = equipmentDto.ImageUrl;
                equipment.Description = equipmentDto.Description;

                _context.Entry(equipment).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }, $"Updated equipment with ID: {id}");
        }

        // DELETE: api/Equipment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            return await HandleAsync(async () =>
            {
                var equipment = await _context.Equipment.FindAsync(id);
                if (equipment == null)
                {
                    return NotFound();
                }

                _context.Equipment.Remove(equipment);
                await _context.SaveChangesAsync();

                return NoContent();
            }, $"Deleted equipment with ID: {id}");
        }

        // GET: api/Equipment/Available
        [HttpGet("Available")]
        public async Task<IActionResult> GetAvailableEquipment()
        {
            return await HandleAsync(async () =>
            {
                var availableEquipment = await _context.Equipment
                    .Where(e => e.IsAvailable)
                    .Select(e => new EquipmentDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Type = e.Type,
                        Size = e.Size,
                        RentalPricePerHour = e.RentalPricePerHour,
                        IsAvailable = e.IsAvailable,
                        ImageUrl = e.ImageUrl,
                        Description = e.Description
                    })
                    .ToListAsync();

                return availableEquipment;
            }, "Retrieved all available equipment");
        }
    }
}