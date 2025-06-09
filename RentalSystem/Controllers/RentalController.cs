// Controllers/RentalController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalSystem.Data;
using RentalSystem.DTOs;
using RentalSystem.Models;

namespace RentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : BaseController
    {
        public RentalController(RentalDbContext context, ILogger<RentalController> logger) 
            : base(context, logger)
        {
        }

        // GET: api/Rental
        [HttpGet]
        public async Task<IActionResult> GetAllRentals()
        {
            return await HandleAsync(async () =>
            {
                var rentals = await _context.Rentals
                    .Include(r => r.Customer)
                    .Include(r => r.RentalEquipments)
                    .ThenInclude(re => re.Equipment)
                    .Select(r => new RentalDto
                    {
                        Id = r.Id,
                        CustomerId = r.CustomerId,
                        CustomerName = $"{r.Customer.FirstName} {r.Customer.LastName}",
                        RentalStart = r.RentalStart,
                        RentalEnd = r.RentalEnd,
                        TotalCost = r.TotalCost,
                        IsPaid = r.IsPaid,
                        Status = r.Status,
                        Equipments = r.RentalEquipments.Select(re => new RentalEquipmentDto
                        {
                            EquipmentId = re.EquipmentId,
                            EquipmentName = re.Equipment.Name,
                            Quantity = re.Quantity,
                            UnitPriceAtRental = re.UnitPriceAtRental
                        }).ToList()
                    })
                    .ToListAsync();

                return rentals;
            }, "Retrieved all rentals");
        }

        // GET: api/Rental/Active
        [HttpGet("Active")]
        public async Task<IActionResult> GetActiveRentals()
        {
            return await HandleAsync(async () =>
            {
                var activeRentals = await _context.Rentals
                    .Where(r => r.Status == "active")
                    .Include(r => r.Customer)
                    .Include(r => r.RentalEquipments)
                    .ThenInclude(re => re.Equipment)
                    .Select(r => new RentalDto
                    {
                        Id = r.Id,
                        CustomerId = r.CustomerId,
                        CustomerName = $"{r.Customer.FirstName} {r.Customer.LastName}",
                        RentalStart = r.RentalStart,
                        RentalEnd = r.RentalEnd,
                        TotalCost = r.TotalCost,
                        IsPaid = r.IsPaid,
                        Status = r.Status,
                        Equipments = r.RentalEquipments.Select(re => new RentalEquipmentDto
                        {
                            EquipmentId = re.EquipmentId,
                            EquipmentName = re.Equipment.Name,
                            Quantity = re.Quantity,
                            UnitPriceAtRental = re.UnitPriceAtRental
                        }).ToList()
                    })
                    .ToListAsync();

                return activeRentals;
            }, "Retrieved active rentals");
        }

        // POST: api/Rental
        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] RentalCreateDto rentalDto)
        {
            return await HandleAsync(async () =>
            {
                // Проверяем доступность оборудования
                foreach (var item in rentalDto.Equipments)
                {
                    var equipment = await _context.Equipment.FindAsync(item.EquipmentId);
                    if (equipment == null || !equipment.IsAvailable)
                    {
                        return BadRequest($"Equipment with ID {item.EquipmentId} is not available");
                    }
                }

                // Создаем прокат
                var rental = new Rental
                {
                    CustomerId = rentalDto.CustomerId,
                    RentalStart = DateTime.UtcNow,
                    Status = "active",
                    IsPaid = false,
                    TotalCost = 0 // Пока 0, рассчитаем ниже
                };

                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();

                // Добавляем оборудование в прокат
                decimal totalCost = 0;
                foreach (var item in rentalDto.Equipments)
                {
                    var equipment = await _context.Equipment.FindAsync(item.EquipmentId);
                    equipment.IsAvailable = false;

                    var rentalEquipment = new RentalEquipment
                    {
                        RentalId = rental.Id,
                        EquipmentId = item.EquipmentId,
                        Quantity = item.Quantity,
                        UnitPriceAtRental = equipment.RentalPricePerHour
                    };

                    totalCost += equipment.RentalPricePerHour * item.Quantity;
                    _context.RentalEquipment.Add(rentalEquipment);
                }

                // Обновляем общую стоимость
                rental.TotalCost = totalCost;
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rental);
            }, "Created new rental");
        }

        // PUT: api/Rental/Complete/5
        [HttpPut("Complete/{id}")]
        public async Task<IActionResult> CompleteRental(int id)
        {
            return await HandleAsync(async () =>
            {
                var rental = await _context.Rentals
                    .Include(r => r.RentalEquipments)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (rental == null)
                {
                    return NotFound();
                }

                if (rental.Status != "active")
                {
                    return BadRequest("Rental is not active");
                }

                // Освобождаем оборудование
                foreach (var item in rental.RentalEquipments)
                {
                    var equipment = await _context.Equipment.FindAsync(item.EquipmentId);
                    equipment.IsAvailable = true;
                }

                // Обновляем прокат
                rental.RentalEnd = DateTime.UtcNow;
                rental.Status = "completed";
                await _context.SaveChangesAsync();

                return NoContent();
            }, $"Completed rental with ID: {id}");
        }

        // GET: api/Rental/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRental(int id)
        {
            return await HandleAsync(async () =>
            {
                var rental = await _context.Rentals
                    .Include(r => r.Customer)
                    .Include(r => r.RentalEquipments)
                    .ThenInclude(re => re.Equipment)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (rental == null)
                {
                    return NotFound();
                }

                return new RentalDto
                {
                    Id = rental.Id,
                    CustomerId = rental.CustomerId,
                    CustomerName = $"{rental.Customer.FirstName} {rental.Customer.LastName}",
                    RentalStart = rental.RentalStart,
                    RentalEnd = rental.RentalEnd,
                    TotalCost = rental.TotalCost,
                    IsPaid = rental.IsPaid,
                    Status = rental.Status,
                    Equipments = rental.RentalEquipments.Select(re => new RentalEquipmentDto
                    {
                        EquipmentId = re.EquipmentId,
                        EquipmentName = re.Equipment.Name,
                        Quantity = re.Quantity,
                        UnitPriceAtRental = re.UnitPriceAtRental
                    }).ToList()
                };
            }, $"Retrieved rental with ID: {id}");
        }

        // PUT: api/Rental/Pay/5
        [HttpPut("Pay/{id}")]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            return await HandleAsync(async () =>
            {
                var rental = await _context.Rentals.FindAsync(id);
                if (rental == null)
                {
                    return NotFound();
                }

                rental.IsPaid = true;
                await _context.SaveChangesAsync();

                return NoContent();
            }, $"Marked rental with ID: {id} as paid");
        }
    }
}