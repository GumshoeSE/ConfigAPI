using ConfigAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConfigAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _clientAPIKey;
        private readonly string _adminAPIKey;
        private readonly string _serverSecret;

        public AuthService(APIConfig config)
        {
            _clientAPIKey = config.ClientAPIKey;
            _adminAPIKey = config.AdminAPIKey;
            _serverSecret = config.ServerSecret;
        }

        public IResult Login(LoginModel user)
        {
            if (user is null)
                return Results.BadRequest("Invalid client request");

            if (user.APIKey == _clientAPIKey || user.APIKey == _adminAPIKey)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_serverSecret));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var role = user.APIKey == _clientAPIKey ? "Client" : "Admin";
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, role)
                    },
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                var response = new AuthenticatedResponse { Token = tokenString };
                return Results.Ok(response);
            }

            return Results.Unauthorized();
        }
    }
}
