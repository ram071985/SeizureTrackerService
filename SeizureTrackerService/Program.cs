using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Identity;
using SeizureTrackerService.Service;
using SeizureTrackerService.Context;
using SeizureTrackerService.Context.Entities;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration["AllowedOrigins"]
    .Split(';');

builder.Services.AddDbContext<SeizureTrackerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});

builder.Services.AddScoped<ISeizureTrackerContext>(provider =>
    provider.GetRequiredService<SeizureTrackerContext>());

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB")));

builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options => {
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<AppIdentityDbContext>();
// builder.Services.AddIdentityCore<ApplicationUser>(options =>
//     {
//         options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
//     })
//     .AddEntityFrameworkStores<AppIdentityDbContext>()
//     .AddSignInManager<SignInManager<ApplicationUser>>();

builder.Services.Configure<IdentityPasskeyOptions>(options =>
{
    // This MUST match your Azure App Service URL (e.g., myapp.azurewebsites.net)
    options.ServerDomain = builder.Configuration["Authentication:Passkey:Domain"];
    // OPTIONAL: How long the browser waits for the user to scan biometrics
    options.AuthenticatorTimeout = TimeSpan.FromMinutes(3);

    // // OPTIONAL: Standard challenge size is 32 bytes
    options.ChallengeSize = 32;
});

builder.Services.AddScoped<ISeizureTrackerService, SeizureTrackerService.Service.SeizureTrackerService>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "MyAllowSpecificOrigins",
            policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
        );
    }
);

builder.Services.AddSwaggerGen(c =>
{
    const string schemeId = "Bearer";

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Seizure Tracker API", Version = "v1" });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "OAuth2.0 Auth Code with PKCE",
        Name = "oauth2",
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl =
                    new Uri(
                        "https://login.microsoftonline.com/77a231ee-ce66-4265-ab57-611db161f461/oauth2/v2.0/authorize"),
                TokenUrl = new Uri(
                    "https://login.microsoftonline.com/77a231ee-ce66-4265-ab57-611db161f461/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
                {
                    {
                        "https://login.microsoftonline.com/77a231ee-ce66-4265-ab57-611db161f461/oauth2/v2.0/token",
                        "read the api"
                    }
                }
            }
        }
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(schemeId, document),
            ["https://login.microsoftonline.com/77a231ee-ce66-4265-ab57-611db161f461/oauth2/v2.0/token"]
        }
    });
});
// builder.Services.ConfigureSwaggerGen(setup =>
// {
//     setup.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "Weather Forecasts",
//         Version = "v1"
//     });
// });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("MyAllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();