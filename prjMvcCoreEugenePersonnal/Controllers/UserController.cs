using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class UserController : SuperController
    {
        public IActionResult List(CKeywordViewModel vm)
        {
            EugenePowerContext db = new EugenePowerContext();
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
            EugenePowerContext db = new EugenePowerContext();
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            EugenePowerContext db = new EugenePowerContext();
            User user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("List");    
        }
        public IActionResult Edit(int? id)
        {
            if (id == null) 
                return RedirectToAction("List");
            EugenePowerContext db = new EugenePowerContext();
            User u = db.Users.FirstOrDefault(u => u.UserId == id);
            if(u == null)
                return RedirectToAction("List");

            return View(u);
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
            EugenePowerContext db = new EugenePowerContext();
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
    }
}
