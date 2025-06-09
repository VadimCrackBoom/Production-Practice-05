using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalSystem.Data;
using RentalSystem.DTOs;
using RentalSystem.Models;

namespace RentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        public CustomerController(RentalDbContext context, ILogger<CustomerController> logger) 
            : base(context, logger)
        {
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            return await HandleAsync(async () =>
            {
                var customers = await _context.Customers
                    .Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        PhoneNumber = c.PhoneNumber,
                        Email = c.Email,
                        DocumentNumber = c.DocumentNumber,
                        RegistrationDate = c.RegistrationDate
                    })
                    .ToListAsync();

                return customers;
            }, "Retrieved all customers");
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            return await HandleAsync(async () =>
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }

                return new CustomerDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.Email,
                    DocumentNumber = customer.DocumentNumber,
                    RegistrationDate = customer.RegistrationDate
                };
            }, $"Retrieved customer with ID: {id}");
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto customerDto)
        {
            return await HandleAsync(async () =>
            {
                var customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    PhoneNumber = customerDto.PhoneNumber,
                    Email = customerDto.Email,
                    DocumentNumber = customerDto.DocumentNumber,
                    RegistrationDate = DateTime.UtcNow
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
            }, "Created new customer");
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            return await HandleAsync(async () =>
            {
                if (id != customerDto.Id)
                {
                    return BadRequest();
                }

                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                customer.FirstName = customerDto.FirstName;
                customer.LastName = customerDto.LastName;
                customer.PhoneNumber = customerDto.PhoneNumber;
                customer.Email = customerDto.Email;
                customer.DocumentNumber = customerDto.DocumentNumber;

                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }, $"Updated customer with ID: {id}");
        }

        // GET: api/Customer/Search?name=...
        [HttpGet("Search")]
        public async Task<IActionResult> SearchCustomers([FromQuery] string name)
        {
            return await HandleAsync(async () =>
            {
                var customers = await _context.Customers
                    .Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name))
                    .Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        PhoneNumber = c.PhoneNumber,
                        Email = c.Email,
                        DocumentNumber = c.DocumentNumber,
                        RegistrationDate = c.RegistrationDate
                    })
                    .ToListAsync();

                return customers;
            }, $"Searched customers with name: {name}");
        }
    }
}