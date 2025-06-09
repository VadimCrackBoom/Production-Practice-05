using Microsoft.AspNetCore.Mvc;

namespace RentalSystem.DTOs;

public class ReportRequestDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ReportType { get; set; }
}

public class ReportResponseDto : IActionResult
{
    public string Title { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public object Data { get; set; }
    
    public Task ExecuteResultAsync(ActionContext context)
    {
        throw new NotImplementedException();
    }
}