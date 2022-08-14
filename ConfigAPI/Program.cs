using ConfigAPI.Extensions;
using ConfigAPI.Models;
using ConfigAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IConfigService<LSSConfig>, ConfigService<LSSConfig>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddCors();
builder.Services.ConfigureAuthentication();
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

app.MapDelete("/api/lss/{customer}", [Authorize(Roles = "Admin")] async (string customer, IConfigService<LSSConfig> configService)
    => await configService.Remove("LSS", customer));

app.Run();