using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class OrderController : ManagerController
    {
        EugenePower0916Context db = new EugenePower0916Context();
        private IWebHostEnvironment _envior = null;
        public OrderController(IWebHostEnvironment p)
        {
            _envior = p;
        }
        public IActionResult List(CKeywordViewModel vm)
        {
            IEnumerable<Order> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.Orders
                        select p;
            else
                datas = db.Orders.Where(p => p.OrderStatus.Contains(vm.txtKeyword));
            return View(datas);
        }

        public IActionResult OrderShip(int? id)
        {
            Order order = db.Orders.FirstOrDefault(o=> o.OrderId == id);
            order.OrderStatus = "Shipped";
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult Delete(int? id)
        {
            Order order = db.Orders.FirstOrDefault(o => o.OrderId == id);
            order.OrderStatus = "Cancel";
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
