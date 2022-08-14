using ConfigAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConfigAPI.Services
{
    public class AuthService : IAuthService
    {
        public IResult Login(LoginModel user)
        {
            if (user is null)
            {
                return Results.BadRequest("Invalid client request");
            }

            if (user.APIKey == "CNm2p339P05RDjjm2OCqTnO0UFCJuImyZaOLBCPQQGHdEH2YZqeQ7XWp3t1Lnwbyt83oDJsnvA24S3cMgLxAmcqQVjcneXNKLTI4Ueu1AOYRjADPEqy3zloU5zfza7Jh")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, "Client")
                    },
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                var response = new AuthenticatedResponse { Token = tokenString };
                return Results.Ok(response);
            }

            else if (user.APIKey == "R9zBokbgWak6FR8l8TNeZZ4J73dUhWXGJnirtpuJmNCiRmYCduRNDufBJM4MC25xpjWexJwdZVsWGmHAoCTrknBxUR6LlkrNL6cNJ2XuFLmwjTLapLmrM5xRilheMR8O")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, "Admin")
                    },
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                var response = new AuthenticatedResponse { Token = tokenString };
                return Results.Ok(response);
            }
            return Results.Unauthorized();
        }
    }
}
