using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;
using System.Diagnostics;
using System.Text.Json;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return View();
            }
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm)
        {
            User user = (new EugenePowerContext()).Users.FirstOrDefault(
                t => t.Email.Equals(vm.txtAccount) && t.PasswordHash.Equals(vm.txtPassword)
                );
            if (user != null && user.PasswordHash.Equals(vm.txtPassword))
            {
                string json = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER
                    , json);

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}