using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;

namespace prjMvcCoreEugenePersonnal.Controllers
{

    public class ProductController : ManagerController
    {
        EugenePower0916Context db = new EugenePower0916Context();
        private IWebHostEnvironment _envior = null;
        public ProductController(IWebHostEnvironment p)
        {
            _envior = p;
        }
        public IActionResult List(CKeywordViewModel vm)
        {
            IEnumerable<Product> datas = null;

            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.Products
                        select p;
            else
                datas = db.Products.Where(p => p.ProductName.Contains(vm.txtKeyword));
            return View(datas);
        }

        public async Task<IActionResult> Delete(int id)
        {
            // 找到要删除的Product实体
            var product = await db.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // 找到所有引用该Product的ProductPhoto实体
            var productPhotos = db.ProductPhotos.Where(pp => pp.ProductId == id);

            // 删除所有找到的ProductPhoto实体
            db.ProductPhotos.RemoveRange(productPhotos);

            // 删除Product实体
            db.Products.Remove(product);

            // 保存更改到数据库
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //        return RedirectToAction("List");
        //    EugenePower0916Context db = new EugenePower0916Context();
        //    Product prod = db.Products.FirstOrDefault(p => p.ProductId == id);
        //    if (prod != null)
        //    {
        //        db.Products.Remove(prod);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            EugenePower0916Context db = new EugenePower0916Context();
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
            EugenePower0916Context db = new EugenePower0916Context();
            Product p = db.Products.FirstOrDefault(p => p.ProductId == prod.ProductId);
            if (p != null)
            {
                if (prod.Photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _envior.WebRootPath + "/images/Product/" + photoName;
                    prod.Photo.CopyTo(new FileStream(path, FileMode.Create));
                    prod.ProductImagePath = photoName;
                }
                p.ProductName = prod.ProductName;
                p.Description = prod.Description;
                p.StockQuantity = prod.StockQuantity;
                p.Price = prod.Price;
                p.DateAdded = prod.DateAdded;
                p.Classification = prod.Classification;
                p.ProductImagePath = prod.ProductImagePath;


                db.SaveChanges();
            }
            return RedirectToAction("List");
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
                string photoName = Guid.NewGuid().ToString() + ".jpg";
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
                Classification = pWP.Classification,
            };
            db.Products.Add(p);
            // 不要在这里保存更改，等到所有相关的ProductPhoto都添加后再保存

            foreach (var photo in pWP.photos)
            {
                if (photo != null)
                {
                    string photoNameDetail = Guid.NewGuid().ToString() + ".jpg";
                    string path = _envior.WebRootPath + "/images/Product/Detail/" + photoNameDetail;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                    }
                    var proimg = new ProductPhoto
                    {
                        Product = p, // 使用新创建的Product对象p
                        ProductId = p.ProductId, // 使用新创建的Product对象p的ProductId
                        ProductUrl = photoNameDetail,
                    };
                    db.ProductPhotos.Add(proimg);
                }
            }
            // 在添加了所有的ProductPhoto后保存更改
            db.SaveChanges();
            return RedirectToAction("List");
        }

    }
}

