using Microsoft.AspNetCore.Mvc;
using Calculator.DAL;

namespace Calculator.Services.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly ICalculatorRepository _repo;

    public CalculatorController(ICalculatorRepository repo) => _repo = repo;

   [HttpPost("calculate")]
public async Task<IActionResult> Calculate(string expression, double v1, double v2, string type)
{
    try
    {
        // Fix: Pass all 4 arguments: expression, v1, v2, and operation type
        var result = await _repo.CalculateAsync(expression, v1, v2, type);
        
        return Ok(new { 
            Result = result, 
            Status = "Success", 
            Logged = true 
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { Message = ex.Message });
    }
}
}