// Controllers/ReportController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalSystem.Data;
using RentalSystem.DTOs;
using RentalSystem.Models;

namespace RentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : BaseController
    {
        public ReportController(RentalDbContext context, ILogger<ReportController> logger) 
            : base(context, logger)
        {
        }

        // POST: api/Report/Generate
        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateReport([FromBody] ReportRequestDto reportRequest)
        {
            return await HandleAsync(async () =>
            {
                switch (reportRequest.ReportType.ToLower())
                {
                    case "rentals":
                        return await GenerateRentalsReport(reportRequest);
                    case "revenue":
                        return await GenerateRevenueReport(reportRequest);
                    case "equipment":
                        return await GenerateEquipmentUsageReport(reportRequest);
                    default:
                        return BadRequest("Invalid report type");
                }
            }, $"Generated {reportRequest.ReportType} report");
        }

        private async Task<ReportResponseDto> GenerateRentalsReport(ReportRequestDto request)
        {
            var rentals = await _context.Rentals
                .Where(r => r.RentalStart >= request.StartDate && r.RentalStart <= request.EndDate)
                .Include(r => r.Customer)
                .ToListAsync();

            return new ReportResponseDto
            {
                Title = "Rentals Report",
                GeneratedAt = DateTime.UtcNow,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Data = rentals.Select(r => new
                {
                    r.Id,
                    Customer = $"{r.Customer.FirstName} {r.Customer.LastName}",
                    r.RentalStart,
                    r.RentalEnd,
                    r.TotalCost,
                    r.Status
                })
            };
        }

        private async Task<ReportResponseDto> GenerateRevenueReport(ReportRequestDto request)
        {
            var rentals = await _context.Rentals
                .Where(r => r.RentalStart >= request.StartDate && r.RentalStart <= request.EndDate)
                .ToListAsync();

            var totalRevenue = rentals.Sum(r => r.TotalCost);
            var paidRevenue = rentals.Where(r => r.IsPaid).Sum(r => r.TotalCost);
            var unpaidRevenue = totalRevenue - paidRevenue;

            return new ReportResponseDto
            {
                Title = "Revenue Report",
                GeneratedAt = DateTime.UtcNow,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Data = new
                {
                    TotalRevenue = totalRevenue,
                    PaidRevenue = paidRevenue,
                    UnpaidRevenue = unpaidRevenue,
                    RentalCount = rentals.Count
                }
            };
        }

        private async Task<ReportResponseDto> GenerateEquipmentUsageReport(ReportRequestDto request)
        {
            var equipmentUsage = await _context.RentalEquipment
                .Include(re => re.Equipment)
                .Include(re => re.Rental)
                .Where(re => re.Rental.RentalStart >= request.StartDate && re.Rental.RentalStart <= request.EndDate)
                .GroupBy(re => re.Equipment.Name)
                .Select(g => new
                {
                    EquipmentName = g.Key,
                    TotalRentals = g.Count(),
                    TotalHours = g.Sum(re => (re.Rental.RentalEnd - re.Rental.RentalStart).Value.TotalHours),
                    TotalRevenue = g.Sum(re => re.UnitPriceAtRental * re.Quantity * 
                        (decimal)(re.Rental.RentalEnd - re.Rental.RentalStart).Value.TotalHours)
                })
                .ToListAsync();

            return new ReportResponseDto
            {
                Title = "Equipment Usage Report",
                GeneratedAt = DateTime.UtcNow,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Data = equipmentUsage
            };
        }
    }
}