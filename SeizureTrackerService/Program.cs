using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using SeizureTrackerService.Service;
using SeizureTrackerService.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthZPolicy", policyBuilder =>
        policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement()
            { RequiredScopesConfigurationKey = $"AzureAd:Scopes" }));

builder.Services.AddDbContext<ISeizureTrackerContext, SeizureTrackerContext>(options =>
{    
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});

builder.Services.AddScoped<ISeizureTrackerService, SeizureTrackerService.Service.SeizureTrackerService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
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
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();