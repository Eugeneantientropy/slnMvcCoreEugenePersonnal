using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;

namespace prjMvcCoreEugenePersonnal.Controllers
{

    public class ProductController : SuperController
    {
        EugenePowerContext db = new EugenePowerContext();
        private IWebHostEnvironment _envior = null;
        public ProductController(IWebHostEnvironment p)
        {
            _envior = p;
        }
            public IActionResult List(CKeywordViewModel vm)
        {
            EugenePowerContext db = new EugenePowerContext();
            IEnumerable<Product> datas = null;
            if(string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.Products
                           select p;
            else
                datas = db.Products.Where(p => p.ProductName.Contains(vm.txtKeyword));   
            return View(datas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [Route("Product/Create")]
        [HttpPost]
        public IActionResult Create(CProductWrap pWP)
        {
            if (pWP.Photo != null)
            {
                string photoName = Guid.NewGuid().ToString()+".jpg";
                string path = _envior.WebRootPath + "/images/Product/" + photoName;
                pWP.Photo.CopyTo(new FileStream(path, FileMode.Create));
                pWP.ProductImagePath = photoName;
            }
            Product p = new Product()
            {
                ProductName = pWP.ProductName,
                Description = pWP.Description,
                Price = pWP.Price,
                StockQuantity = pWP.StockQuantity,
                DateAdded = pWP.DateAdded,
                ProductImagePath = pWP.ProductImagePath,

            };
            db.Products.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            EugenePowerContext db = new EugenePowerContext();
            Product prod = db.Products.FirstOrDefault(p => p.ProductId == id);
            if (prod != null)
            {
                db.Products.Remove(prod);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            EugenePowerContext db = new EugenePowerContext();
            Product p = db.Products.FirstOrDefault(p => p.ProductId == id);
            if (p == null)
                return RedirectToAction("List");
            CProductWrap prodWP = new CProductWrap();
            prodWP.product = p;
            return View(prodWP);
        }
        [HttpPost]
        public IActionResult Edit(CProductWrap prod)
        {
            EugenePowerContext db = new EugenePowerContext();
            Product p = db.Products.FirstOrDefault(p => p.ProductId == prod.ProductId);
            if (p != null)
            {
                if(prod.Photo != null)
                {
                    string photoName = Guid.NewGuid().ToString()+".jpg";
                    string path = _envior.WebRootPath + "/images/Product/" + photoName;
                    prod.Photo.CopyTo(new FileStream(path, FileMode.Create));
                    prod.ProductImagePath = photoName;
                }
                p.ProductName = prod.ProductName;
                p.Description = prod.Description;
                p.StockQuantity = prod.StockQuantity;
                p.Price = prod.Price;
                p.DateAdded = prod.DateAdded;
                p.ProductImagePath = prod.ProductImagePath;
                

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
