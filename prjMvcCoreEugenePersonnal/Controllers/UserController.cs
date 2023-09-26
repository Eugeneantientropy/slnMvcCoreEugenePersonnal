using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment _enviro;
        private readonly EmailService _emailService;

        public UserController(IWebHostEnvironment p, EmailService emailService)
        {
            _enviro = p;
            _emailService = emailService;
        }
        //private readonly EugenePower0916Context _context;
        //public UserController(EugenePower0916Context context)
        //{
        //    _context = context;
        //}
        public IActionResult List(CKeywordViewModel vm)
        {
            EugenePower0916Context db = new EugenePower0916Context();
            IEnumerable<User> users = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                users = from u in db.Users
                        select u;
            }
            else
            {
                users = db.Users.Where(c => c.FullName.Contains(vm.txtKeyword)
                        || c.Username.Contains(vm.txtKeyword)
                        || c.Email.Contains(vm.txtKeyword));
            }

            return View(users.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(User u)
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

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            EugenePower0916Context db = new EugenePower0916Context();
            User user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public IActionResult UserInfoEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");

            EugenePower0916Context db = new EugenePower0916Context();
            User u = db.Users.FirstOrDefault(u => u.UserId == id);

            //var orderItems = (from order in db.Orders
            //                  join orderItem in db.OrderItems on order.OrderId equals orderItem.OrderId
            //                  where order.UserId == u.UserId
            //                  select orderItem).ToList();
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
                ViewBag.OrderItems = orderDetails;
            }
            else
            {
                ViewBag.OrderItems = new List<OrderItem>(); // Set it to an empty list instead of null to avoid NullReferenceException
            }

            return View(u);
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


        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            EugenePower0916Context db = new EugenePower0916Context();
            User u = db.Users.FirstOrDefault(u => u.UserId == id);
            if (u == null)
                return RedirectToAction("List");

            return View(u);
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
            EugenePower0916Context db = new EugenePower0916Context();
            User userDb = db.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (userDb != null)
            {
                userDb.Username = user.Username;
                userDb.FullName = user.FullName;
                userDb.Email = user.Email;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public IActionResult ForgetPwd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPwd(string userEmail)
        {

            EugenePower0916Context db = new EugenePower0916Context();



            User user = db.Users.FirstOrDefault(t => t.Email == userEmail);

            if (user == null)
            {
                CAlertMessage resultb = new CAlertMessage(CDictionary.Danger, "此信箱沒有註冊過");
                TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(resultb);
            }
            else
            {
                string randomPassword = RandomPasswordGenerator.GeneratePassword(12); // 生成12个字符的密码
                //Console.WriteLine(randomPassword);
                CAlertMessage resulta = new CAlertMessage(CDictionary.Success, "重製密碼成功");
                TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(resulta);

                // 寄送郵件
                //_emailService.SendEmail(目標信箱, "信件標題", $"您的新密碼是：{randomPassword}");
                _emailService.SendEmail(userEmail, "Flash Bean 會員您好", $"您的新密碼是：{randomPassword}");

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(randomPassword);
                db.SaveChanges();


            }

            return View();
        }
    }
}
