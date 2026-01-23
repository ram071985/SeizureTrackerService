using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // Step 1: Client calls this to get the biometric challenge
    [HttpGet("passkey-options")]
    public async Task<IActionResult> GetPasskeyOptions([FromQuery] string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        // Internal .NET 10 method to generate WebAuthn challenge
        var options = await _signInManager.MakePasskeyRequestOptionsAsync(user);
        return Ok(options);
    }

    // Step 2: Client sends the biometric result here
    [HttpPost("passkey-login")]
    public async Task<IActionResult> PasskeyLogin([FromBody] string credentialJson)
    {
        // Built-in .NET 10 method to verify the biometric credential
        var result = await _signInManager.PasskeySignInAsync(credentialJson);

        if (result.Succeeded)
        {
            // CUSTOM ACCESSIBILITY LOGIC:
            // Since the user just logged in via FaceID/TouchID, we can 
            // automatically check if they need medical assistance.
            return Ok(new { Message = "Login successful", Status = "Ready" });
        }

        return Unauthorized();
    }
}
