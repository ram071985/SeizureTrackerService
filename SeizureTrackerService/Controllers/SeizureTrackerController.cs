using Microsoft.AspNetCore.Mvc;
using SeizureTrackerService.Service;
using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Controllers;

[Route("[controller]")]
[ApiController]
public class SeizureTrackerController(ILogger<SeizureTrackerController> log, IConfiguration config, ISeizureTrackerService seizureTrackerService) : Controller
{

    private readonly ILogger<SeizureTrackerController> _log = log;
    private readonly IConfiguration _config = config;
    private readonly ISeizureTrackerService _seizureTrackerService = seizureTrackerService;
    
    [HttpPost]
    public async Task AddSeizureLog([FromBody] SeizureFormDto form)
    {
        try
        {
            await _seizureTrackerService.AddActivityLog(form);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }
}