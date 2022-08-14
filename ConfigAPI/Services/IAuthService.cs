using ConfigAPI.Models;

namespace ConfigAPI.Services
{
    public interface IAuthService
    {
        IResult Login(LoginModel user);
    }
}
