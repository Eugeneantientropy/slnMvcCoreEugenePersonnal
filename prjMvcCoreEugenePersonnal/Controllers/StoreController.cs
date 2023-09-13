using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class StoreController : SuperController
    {
        EugenePowerContext db =  new EugenePowerContext();
        private IWebHostEnvironment _envior = null;

        public StoreController(IWebHostEnvironment envior)
        {
            _envior = envior;
        }

        public IActionResult Index(CKeywordViewModel vm)
        {
            IEnumerable<Product> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.Products
                        select p;
            else
                datas = db.Products.Where(p => p.ProductName.Contains(vm.txtKeyword));
            return View(datas);
        }
    }
}
