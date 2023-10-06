using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vs.Pm.Pm.Db;
using Vs.Pm.Pm.Db.Models;
using Vs.Pm.Web.Data.SharedService;

namespace Vs.Pm.Web.Data.Account
{
    public class AccountController : Controller
    {
        public AccountController(VsPmContext dbContext, SecurityService securityService)
        {
            vsPmContext = dbContext;
            Service = securityService;
        }

        public VsPmContext vsPmContext { get; set; }
        public SecurityService Service { get; set; }

        [HttpPost]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (!String.IsNullOrEmpty(name))
            {
                var user = GetUser(name, password);
                if (user != null)
                {
                    //string role = GetBenutzerSoftwaresystemRoleById(user.BenutzerId, Globals.SystemId);
                    //var standort = VsPmContext.GetStandortById(user.AlfaStandortNr);
                    await Service.LoginAsync(user, Globals.UserList.FirstOrDefault(x=>x.RoleId == user.RoleId).RoleName);
                    return Json(new { message = "Correct login details", status = 1 });

                    // return RedirectToPage("/_Host");
                }
            }
            return Json(new { message = "Invalid login details", status = 0 });
            //return RedirectToPage("/Account/_Login");
        }
        public async Task<IActionResult> RedirectToHost(int t) => await Task.FromResult(LocalRedirect($"~/"));
        public async Task<IActionResult> Logout()
        {
            await Service.Logout();
            return RedirectToPage("/Account/_Login");
        }
        private User GetUser(string login, string password)
        {
            return vsPmContext.Login(login, password);
        }

    }
}
