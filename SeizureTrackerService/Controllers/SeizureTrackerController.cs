using Microsoft.AspNetCore.Mvc;

namespace SeizureTrackerService.Controllers;

[Route("[controller]")]
[ApiController]
public class SeizureTrackerController(ILogger<SeizureTrackerController> log, IConfiguration config) : Controller
{

    private readonly ILogger<SeizureTrackerController> _log = log;
    private readonly IConfiguration _config = config;
    
    [HttpPost]
    public async Task<SeizureFormDto> AddSeizureLog([FromBody] SeizureFormDto form)
    {
        try
        {
            var log = await _seizureTrackerService.AddRecord(form);

            return log;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }
}