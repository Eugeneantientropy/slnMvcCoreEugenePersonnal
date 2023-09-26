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
    public class EmployeeController : Controller
    {
        private readonly EugenePower0916Context _context;
        public EmployeeController(EugenePower0916Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee e)
        {
            EugenePower0916Context eugenePower0916Context = new EugenePower0916Context();
            Employee employee = (new EugenePower0916Context()).Employees.FirstOrDefault(t => t.EmployeeEmail.Equals(e.EmployeeEmail));
            if (employee != null)
            {
                //此Email已註冊過
                return View();
            }
            if (!e.EmployeeEmail.Contains("@"))
            {
                //驗證是否包含@
                return View();
            }
            if (e.EmployeePassword.Length < 8 || !Regex.IsMatch(e.EmployeePassword, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@#$%^&*!])[A-Za-z\d@#$%^&*!]*$"))
            {

                //驗證密碼是否小於8及有無包含英文,數字,特殊符號
                return View();
            }

            e.EmployeePassword = BCrypt.Net.BCrypt.HashPassword(e.EmployeePassword);
            e.EmployeeCancel = false;
            eugenePower0916Context.Employees.Add(e);
            eugenePower0916Context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
