using Microsoft.AspNetCore.Mvc;
using RentalSystem.Data;

namespace RentalSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly RentalDbContext _context;
        protected readonly ILogger _logger;

        public BaseController(RentalDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        protected async Task<IActionResult> HandleAsync<T>(Func<Task<T>> action, string successMessage) where T : class
        {
            try
            {
                var result = await action();
                _logger.LogInformation(successMessage);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        protected async Task<IActionResult> HandleAsync(Func<Task<IActionResult>> action, string successMessage)
        {
            try
            {
                var result = await action();
                _logger.LogInformation(successMessage);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
    }
}