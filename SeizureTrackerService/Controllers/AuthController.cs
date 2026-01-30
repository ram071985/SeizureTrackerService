using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeizureTrackerService.Context.Entities;
using SeizureTrackerService.Service;

namespace SeizureTrackerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISeizureTrackerService _seizureTrackerService;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ISeizureTrackerService seizureTrackerService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _seizureTrackerService = seizureTrackerService;
    }
    [HttpGet("info")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            // Manually check if the user is authenticated via the cookie
            if (User.Identity?.IsAuthenticated != true)
            {
                // Return a successful response indicating "Not Logged In"
                return Ok(UserInfoResponse.Anonymous()); 
            }
            
            // 1. Get the current user from the ClaimsPrincipal (User property)
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized("User session no longer valid.");
            }

            // 2. Fetch additional data like roles
            var roles = await _userManager.GetRolesAsync(user);

            var info = new UserInfoResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                IsEmailConfirmed = user.EmailConfirmed,
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false, 
                Roles = roles.ToList(),
                // Extract standard claims (e.g., NameIdentifier)
                Claims = User.Claims.ToDictionary(c => c.Type, c => c.Value)
            };
           return Ok(ServiceResult<UserInfoResponse>.Ok(info));
           // return Content(JsonSerializer.Serialize(info), "application/json");
        }
        catch (Exception ex)
        {
            return Problem("An internal error occurred while fetching user data.");
        }
    }
    // 1. Initial Registration (Email/Password)
    [HttpPost("register")]
    // [Authorize(Roles = "WhitelistedUser")] // This checks the JWT you issued
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            // 1. YOUR WHITELIST CHECK
            var isAllowed = await _seizureTrackerService.CheckWhiteListSproc(request.Email);
            
            if (!isAllowed) return Forbid(); // Stop unauthorized sign-up here
            
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "WhitelistedUser");
                
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

    // This is the route for AccountClient.LoginWithPasswordAsync
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            // 1. Validate Input
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            // 2. Attempt Sign-In
            // isPersistent: true (Remember Me) 
            // lockoutOnFailure: true (Security best practice for 2026)
            var result = await _signInManager.PasswordSignInAsync(
                request.Email,
                request.Password,
                isPersistent: true,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Login successful" });
            }

            if (result.IsLockedOut)
            {
                return StatusCode(423, "Account is locked. Please try again later.");
            }

            if (result.RequiresTwoFactor)
            {
                return Ok(new { RequiresTwoFactor = true });
            }

            // 3. Fail gracefully if credentials don't match
            return Unauthorized("Invalid email or password.");
        }
        catch (DbUpdateException ex)
        {
            // Database connection or timeout issues
            return Problem("A database error occurred. Please try again later.");
        }
        catch (Exception ex)
        {
            // Catch-all for unexpected infrastructure failures
            return Problem("An unexpected error occurred on the server.");
        }
    }

    // Step 1: Client calls this to get the biometric challenge
    [HttpGet("passkey-options")]
    [AllowAnonymous] // Ensure the provider can call this during initial boot
    public async Task<IActionResult> GetPasskeyOptions([FromQuery] string? email)
    {
        try
        {
            // 1. Identify the user if an email is provided
            // If email is null or empty, we treat the request as "Email-less"
            var user = string.IsNullOrWhiteSpace(email) 
                ? null 
                : await _userManager.FindByEmailAsync(email);

            // 2. Generate the login challenge
            // .NET 10: Passing 'null' creates a Discoverable Credential request
            // which triggers the biometric prompt for ANY stored passkey on the device.
            string optionsJson = await _signInManager.MakePasskeyRequestOptionsAsync(user!);

            // 3. Return as raw JSON
            return Content(optionsJson, "application/json");
        }
        catch (Exception ex)
        {
            // Log the error for medical app audit trails
            return Problem("An internal error occurred while initiating biometric login.");
        }
    }

    [HttpGet("register-options")]
    [Authorize] // User must be logged in (via password or other) to add a biometric factor
    public async Task<IActionResult> GetRegisterPasskeyOptions()
    {
        // Check SQL Server Whitelist
        // var isWhitelisted = await _context.Whitelist
        //     .AnyAsync(w => w.Email == email && w.IsActive);
        //
        // if (!isWhitelisted) 
        //     return Forbid("You are not authorized to create an account.");

        // If allowed, continue with FIDO2/Passkey options generation...
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
    
    [HttpPost("register-passkey-finish")]
    [Authorize]
    public async Task<IActionResult> RegisterPasskeyFinish([FromBody] string credentialJson)
    {
        try
        {
            // 1. VERIFICATION (SignInManager)
            //  // In the GA release, this method automatically locates the challenge
            //  // associated with the current user's session.
            var attestationResult = await _signInManager.PerformPasskeyAttestationAsync(credentialJson);

            if (!attestationResult.Succeeded)
            {
                return BadRequest($"Verification failed: {attestationResult.Failure?.Message}");
            }
            // 2. PERSISTENCE (UserManager)
            // Use AddOrUpdatePasskeyAsync to save the validated public key.
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
        
            // In the final .NET 10 release, this is AddPasskeyAsync()
            var result = await _userManager.AddOrUpdatePasskeyAsync(user, attestationResult.Passkey);
            
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
        catch (Exception ex)
        {
            return Problem("An unexpected error occurred during passkey registration.");
        }
    }
    
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            // 1. Core Logic: Sign out from the Identity Cookie scheme
            await _signInManager.SignOutAsync();
        
            // 2. Clear other authentication schemes (if using OIDC or external providers)
            // This ensures the browser doesn't immediately 'auto-login' the user back in
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);

            // 3. Return a clean success status
            return Ok(new { Message = "Logged out successfully" });
        }
        catch (InvalidOperationException ex)
        {
            // Occurs if the authentication configuration is missing or invalid
            return Problem("Server authentication misconfigured.", statusCode: 500);
        }
        catch (Exception ex)
        {
            // General catch for unexpected infrastructure issues (DB, Middleware, etc.)
            return Problem("An internal error occurred while ending your session.");
        }
    }

}

public record RegisterRequest(string Email, string Password);

public record LoginRequest(string Email, string Password, bool RememberMe = false);

public class UserInfoResponse
{
    public UserInfoResponse() { } 
    public bool? IsAuthenticated { get; set; }
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public required List<string> Roles { get; set; }
    public Dictionary<string, string> Claims { get; set; } = new();
    public bool HasPasskeys { get; set; }
    
    
    // The helper method
    public static UserInfoResponse Anonymous() 
    {
        return new UserInfoResponse 
        { 
            UserId = string.Empty,
            IsAuthenticated = false, 
            Email = "Anonymous", 
            Roles = new List<string>() 
        };
    }
}

public class ServiceResult<T>(T? data, string? errorMessage = null)
{
    public T? Data { get; } = data;
    public string? ErrorMessage { get; } = errorMessage;
    public bool Success => ErrorMessage == null;

    // Static helper for Success
    public static ServiceResult<T> Ok(T data) 
        => new ServiceResult<T>(data, null);

    // Static helper for Failure
    public static ServiceResult<T> Fail(string message) 
        => new ServiceResult<T>(default, message);
}