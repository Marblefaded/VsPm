using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Vs.Pm.Pm.Db.Models;

namespace Vs.Pm.Web.Data.SharedService
{
    public class SecurityService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SecurityService(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;

        }

        public async Task LoginAsync(User user, string role, string standort = "")
        {
            try
            {
                await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }

            var claims = new[] { new Claim(ClaimTypes.Name, user.Login),
                new Claim("UserId",user.UserId.ToString()),
                new Claim("Nachname",user.PersonName),
                new Claim("Vorname", user.PersonSurname),
                new Claim(ClaimTypes.Role, role) };

            var identity = new ClaimsIdentity(claims, ConstField.CookieScheme);

            await httpContextAccessor.HttpContext.SignInAsync(
                ConstField.CookieScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true
                });
        }

        public async Task Logout()
        {
            await httpContextAccessor.HttpContext.SignOutAsync();

        }


    }
}
