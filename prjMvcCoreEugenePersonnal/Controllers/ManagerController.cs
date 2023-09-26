using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using prjMvcCoreEugenePersonnal.Models;
using System.Text.Json;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class ManagerController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)||CLogginUsercs.LoggedUser!= "employee")
            {
                CAlertMessage result = new CAlertMessage(CDictionary.Danger, "僅限管理員權限進入");
                TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(result);
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Login"
                }));
            }
        }

    }
}
