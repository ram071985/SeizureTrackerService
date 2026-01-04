using Microsoft.AspNetCore.Mvc;
using SeizureTrackerService.Service;
using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Controllers;

[ApiController]
[Route("[controller]")]
public class SeizureTrackerController(ILogger<SeizureTrackerController> log, IConfiguration config, ISeizureTrackerService seizureTrackerService) : Controller
{

    private readonly ILogger<SeizureTrackerController> _log = log;
    private readonly IConfiguration _config = config;
    private readonly ISeizureTrackerService _seizureTrackerService = seizureTrackerService;
    
    [HttpPost]
    public async Task AddSeizureLog([FromBody] SeizureActivityDetailDTO log)
    {
        try
        {
            await _seizureTrackerService.AddActivityLog(log);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }
}