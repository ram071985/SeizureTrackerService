using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    // 1. Initial Registration (Email/Password)
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
            
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            
            return BadRequest(result.Errors);
        }
        catch (DbUpdateException ex)
        {
            // Handle specific database issues (e.g., unique constraint violations not caught by Identity)
            
            return Problem("A database error occurred. Please try again later.");
        }
        catch (Exception ex)
        {
            // Handle truly exceptional, unforeseen errors
            
            return Problem("An internal server error occurred.");
        }

    }

    // Step 1: Client calls this to get the biometric challenge
    [HttpGet("passkey-options")]
    public async Task<IActionResult> GetPasskeyOptions([FromQuery] string email)
    {
        try
        {
            // 1. Validate input
            if (string.IsNullOrEmpty(email)) return BadRequest("Email is required.");
            
            var user = await _userManager.FindByEmailAsync(email);
            
            // SECURITY TIP: In a production app, if the user is null, 
            // consider returning a "dummy" challenge to prevent account enumeration.
            if (user == null) return NotFound("User not found.");
            
            // 3. Generate the login challenge (assertion options)
            // .NET 10: This returns a JSON string representing PasskeyRequestOptions
            string optionsJson = await _signInManager.MakePasskeyRequestOptionsAsync(user);

            // 4. Return as raw JSON
            // Using Content prevents double-serialization that breaks the browser API
            return Content(optionsJson, "application/json");
        }
        catch (InvalidOperationException ex)
        {
            // Occurs if the Identity store or DB schema isn't set up for passkeys
            return Problem("Biometric login is currently unavailable.");
        }
        catch (Exception ex)
        {
            return Problem("An internal error occurred.");
        }
    }

    [HttpGet("register-passkey-options")]
    [Authorize] // User must be logged in (via password or other) to add a biometric factor
    public async Task<IActionResult> GetRegisterPasskeyOptions()
    {
        try
        {
            // 1. Identify the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var userEntity = new PasskeyUserEntity
            {
                Id = await _userManager.GetUserIdAsync(user),
                Name = user.Email!,
                DisplayName = user.Email ?? "User"
            };
            
            // 3. Generate the creation options
            // This internally handles the challenge generation and session state
            var options = await _signInManager.MakePasskeyCreationOptionsAsync(userEntity);

            // 4. Return as JSON (WebAuthn requires specific formatting handled by AsJson())
            return Content(options, "application/json");
        }
        catch (InvalidOperationException ex)
        {
            // Thrown if the UserStore does not support passkeys (e.g., missing AspNetUserPasskeys table)
          
            return Problem("Passkey registration is currently unavailable on the server.");
        }
        catch (Exception ex)
        {
            // General infrastructure or unexpected failures
            
            return Problem("An internal error occurred. Please try again later.");
        }
    }

    // Step 2: Client sends the biometric result here
    [HttpPost("passkey-login")]
    public async Task<IActionResult> PasskeyLogin([FromBody] string credentialJson)
    {
        try
        {
            // Built-in .NET 10 method to verify the biometric credential
            var result = await _signInManager.PasskeySignInAsync(credentialJson);

            if (result.Succeeded)
                return Ok(new { Message = "Login successful", Status = "Ready" });
            if (result.IsLockedOut)
                return StatusCode(423, "Account is locked. Please try again later.");
            if (result.RequiresTwoFactor)
                return Ok(new { RequiresTwoFactor = true });

            // General failure (e.g., biometric mismatch or invalid credential)
            return Unauthorized("Biometric authentication failed.");
        }
        catch (Exception ex)
        {
            // Handle unexpected server-side errors (DB connectivity, etc.)
            return Problem("An unexpected error occurred on the server.");
        }
    }
}

public record RegisterRequest(string Email, string Password);