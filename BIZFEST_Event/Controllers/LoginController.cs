using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BIZFEST_Event.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _LoginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            _LoginRepository=loginRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserLogin(AdminLogin AdminLoginModel)
        {
            bool IsSuccess = await _LoginRepository.LoginUser(AdminLoginModel.UserName, AdminLoginModel.Pasword);
            
            if (IsSuccess)
            {
                HttpContext.Session.SetString("UserName", AdminLoginModel.UserName);
                return RedirectToAction("Index", "Admin");
            }
            else
                return RedirectToAction("Index");

        }
    }
}
