using Microsoft.AspNetCore.Mvc;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
