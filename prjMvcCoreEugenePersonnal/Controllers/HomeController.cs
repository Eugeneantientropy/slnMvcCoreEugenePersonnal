using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class HomeController : Controller
    {
 
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            EugenePower0916Context db = new EugenePower0916Context();
            return View(await db.Posts.OrderByDescending(p => p.DatePosted).Take(3).ToListAsync());
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User u)
        {
            User user = (new EugenePower0916Context()).Users.FirstOrDefault(t => t.Email.Equals(u.Email));
            if (user != null)
            {
                //此Email已註冊過
                return View();
            }
            if (!u.Email.Contains("@"))
            {
                //驗證是否包含@
                return View();
            }
            if (u.PasswordHash.Length < 8 || !Regex.IsMatch(u.PasswordHash, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@#$%^&*!])[A-Za-z\d@#$%^&*!]*$"))
            {

                //驗證密碼是否小於8及有無包含英文,數字,特殊符號
                return View();
            }

            EugenePower0916Context db = new EugenePower0916Context();
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(u.PasswordHash);
            u.DateRegistered = DateTime.Now;
            u.LastLogin = DateTime.Now;
            db.Users.Add(u);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm)
        {
            User user = (new EugenePower0916Context()).Users.FirstOrDefault(
                t => t.Email.Equals(vm.txtAccount));
            if (user != null)
            {
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(vm.txtPassword, user.PasswordHash);
                if (isValidPassword)
                {
                    string json = JsonSerializer.Serialize(user);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);

                    if (HttpContext.Session.TryGetValue("CurrentPageUrl", out var urlBytes) && urlBytes != null)
                    {
                        var currentPageUrl = System.Text.Encoding.Default.GetString(urlBytes);
                        HttpContext.Session.Remove("CurrentPageUrl");
                        CAlertMessage resultc = new CAlertMessage(CDictionary.Success, "登入成功");
                        TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(resultc);
                        CLogginUsercs.LoggedUser = "user";
                        return Redirect(currentPageUrl);
                    }

                    // Add a default redirect here, for instance to the home page
                    CAlertMessage resulta = new CAlertMessage(CDictionary.Success, "登入成功");
                    TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(resulta);
                    CLogginUsercs.LoggedUser = user.Username;
                    CLogginUsercs.LoggenUserId = user.UserId;


                    return RedirectToAction("Index", "Home");
                }
            }

            // If we reached here, it means the login failed


            // Redirecting to the login page with an error message
            CAlertMessage result = new CAlertMessage(CDictionary.Danger, "帳號或密碼錯誤");
            TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(result);
            return RedirectToAction("Login", "Home");
        }
        public IActionResult EmployeeLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EmployeeLogin(CLoginViewModel vm)
        {
            Employee employee = (new EugenePower0916Context()).Employees.FirstOrDefault(
                t => t.EmployeeEmail.Equals(vm.txtAccount));
            if (employee != null)
            {
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(vm.txtPassword, employee.EmployeePassword);
                if (isValidPassword)
                {
                    string json = JsonSerializer.Serialize(employee);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);

                    if (HttpContext.Session.TryGetValue("CurrentPageUrl", out var urlBytes) && urlBytes != null)
                    {
                        var currentPageUrl = System.Text.Encoding.Default.GetString(urlBytes);
                        HttpContext.Session.Remove("CurrentPageUrl");
                        CAlertMessage resultc = new CAlertMessage(CDictionary.Success, "登入成功");
                        TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(resultc);
                        CLogginUsercs.LoggedUser = "employee";
                        return Redirect(currentPageUrl);
                    }

                    // Add a default redirect here, for instance to the home page
                    CAlertMessage resulta = new CAlertMessage(CDictionary.Success, "登入成功");
                    TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(resulta);
                    CLogginUsercs.LoggedUser = "employee";
                    CLogginUsercs.LoggenUserId = employee.EmployeeId;


                    return RedirectToAction("Index", "Home");
                }
            }

            // If we reached here, it means the login failed
            // Redirecting to the login page with an error message
            CAlertMessage result = new CAlertMessage(CDictionary.Danger, "帳號或密碼錯誤");
            TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(result);
            return RedirectToAction("Login", "Home");
        }

        public IActionResult logout()
        {  //把session清空
            HttpContext.Session.Remove(CDictionary.SK_LOGINED_USER);
            _ = CDictionary.SK_LOGINED_USER == null;
            CAlertMessage resulta = new CAlertMessage(CDictionary.Success, "登出成功");
            TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(resulta);
            CLogginUsercs.LoggedUser = "";

            return RedirectToAction("Login", "Home");
        }
        public IActionResult About() { return View(); }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult UserInfoEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");

            using (var db = new EugenePower0916Context())
            {
                User u = db.Users.FirstOrDefault(u => u.UserId == id);

                string currentUsername = User.Identity.Name;

                ViewBag.CurrentUsername = currentUsername; // 将用户名传递到视图

                var orderDetails = from order in db.Orders
                                   where order.UserId == u.UserId
                                   join orderItem in db.OrderItems on order.OrderId equals orderItem.OrderId
                                   join product in db.Products on orderItem.ProductId equals product.ProductId
                                   select new
                                   {
                                       order.OrderId,
                                       order.OrderStatus,
                                       orderItem.Quantity,
                                       product.ProductName,
                                       product.Price
                                   };

                if (orderDetails.Any())
                {
                    ViewBag.OrderDetails = orderDetails.ToList();
                }
                else
                {
                    ViewBag.OrderDetails = new List<OrderItem>();
                }

                return View(u);
            }
        }


        [HttpPost]
        public IActionResult UserInfoEdit(User user)
        {
            using (var db = new EugenePower0916Context())
            {
                User userDb = db.Users.FirstOrDefault(u => u.UserId == user.UserId);

                if (userDb != null)
                {
                    userDb.Username = user.Username;
                    userDb.FullName = user.FullName;
                    userDb.Email = user.Email;
                    userDb.Address = user.Address;
                    userDb.Phone = user.Phone;
                    db.SaveChanges();

                    //var orderItems = (from order in db.Orders
                    //                  join orderItem in db.OrderItems on order.OrderId equals orderItem.OrderId
                    //                  where order.UserId == user.UserId
                    //                  select orderItem).ToList();
                    var orderDetails = from order in db.Orders
                                       where order.UserId == user.UserId
                                       join orderItem in db.OrderItems on order.OrderId equals orderItem.OrderId
                                       join product in db.Products on orderItem.ProductId equals product.ProductId
                                       select new
                                       {
                                           order.OrderId,
                                           orderItem.Quantity,
                                           product.ProductName,
                                           product.Price
                                       };

                    ViewBag.OrderDetails = orderDetails.ToList();



                    if (orderDetails.Any())
                    {
                        ViewBag.OrderDetails = orderDetails.ToList();

                    }
                    else
                    {
                        ViewBag.OrderDetails = null;
                        return RedirectToAction("Index", "Home");

                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "User not found.";
                    return View("Error");
                }
            }
            //返回包含更新后的用户信息的视图
            return View(user);
        }
        public IActionResult Chat()
        {
            return View();
        }
    }
}