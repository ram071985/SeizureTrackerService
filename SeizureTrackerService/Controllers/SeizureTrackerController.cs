using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SeizureTrackerService.Constants;
using SeizureTrackerService.Service;
using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeizureTrackerController(ILogger<SeizureTrackerController> log, IConfiguration config, ISeizureTrackerService seizureTrackerService) : Controller
{

    private readonly ILogger<SeizureTrackerController> _log = log;
    private readonly IConfiguration _config = config;
    private readonly ISeizureTrackerService _seizureTrackerService = seizureTrackerService;

    [HttpGet(ApiRoutes.GetHeaders)]
    public async Task<string> GetSeizureActivityHeaders()
    {
        try
        {
            var headers = await _seizureTrackerService.GetSeizureActivityHeaders();
            
            return JsonSerializer.Serialize(headers);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            
            throw;
        }
    }
    
    [HttpGet(ApiRoutes.GetDetailsByHeaderId)]
    public async Task<string> GetSeizureActivityDetailsByHeaderId(int headerId)
    {
        try
        {
            var details = await _seizureTrackerService.GetSeizureActivityDetailsByHeaderId(headerId);
            
            return JsonSerializer.Serialize(details);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            
            throw;
        }
    }
    
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