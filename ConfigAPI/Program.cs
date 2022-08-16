using ConfigAPI;
using ConfigAPI.Extensions;
using ConfigAPI.Models;
using ConfigAPI.Services;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
builder.Configuration.AddEnvironmentVariables();

var apiConfig = new APIConfig(); 
builder.Configuration.GetSection("APIConfig").Bind(apiConfig);

builder.Services.AddScoped<IConfigService<LSSConfig>, ConfigService<LSSConfig>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton(apiConfig);

builder.Services.AddCors();
builder.Services.ConfigureAuthentication(apiConfig.ServerSecret, apiConfig.APIUrl);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/auth/login", (LoginModel user, IAuthService authService) => authService.Login(user));

app.MapGet("/api/lss/{customer}", [Authorize(Roles = "Client, Admin")] async (string customer, IConfigService<LSSConfig> configService)
    => await configService.GetAsync("LSS", customer));

app.MapPost("/api/lss/{customer}", [Authorize(Roles = "Admin")] async (LSSConfig config, string customer,
    IConfigService<LSSConfig> configService) => await configService.AddAsync(config, customer, "LSS"));

app.MapDelete("/api/lss/{customer}", [Authorize(Roles = "Admin")] (string customer, IConfigService<LSSConfig> configService)
    => configService.Remove("LSS", customer));

app.Run();