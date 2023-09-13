using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;
using System.Text.Json;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class ShoppingController : SuperController
    {
        EugenePowerContext db = new EugenePowerContext();
        private IWebHostEnvironment _envior = null;

        public ShoppingController(IWebHostEnvironment envior)
        {
            _envior = envior;
        }

        public IActionResult CartView()
        {
            if(!HttpContext.Session.Keys.Contains(CDictionary.SK_PURCAHSED_PRODUCTS_LIST))
                return RedirectToAction("List");
            string json = HttpContext.Session.GetString(CDictionary.SK_PURCAHSED_PRODUCTS_LIST);
            List<CShoppingCartItem> cart = JsonSerializer.Deserialize<List<CShoppingCartItem>>(json)
                as List<CShoppingCartItem>;   
            if(cart == null)
                return RedirectToAction("List");
            return View(cart);
         }
        public IActionResult AddToCart(int? id)
        {
            var data = db.Products.AsQueryable();

            ViewBag.ProductId = id;

            return View();
        }
        [HttpPost]
        public IActionResult AddToCart(CAddToCartViewModel vm)
        {
            EugenePowerContext db = new EugenePowerContext();
            Product prod = db.Products.FirstOrDefault(t => t.ProductId == vm.txtProductID);
            if (prod == null)
                return RedirectToAction("List");
            string json = "";
            List<CShoppingCartItem> cart = null;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_PURCAHSED_PRODUCTS_LIST))
            {
                json = HttpContext.Session.GetString(CDictionary.SK_PURCAHSED_PRODUCTS_LIST);
                cart = JsonSerializer.Deserialize<List<CShoppingCartItem>>(json);
            }
            else
                cart = new List<CShoppingCartItem>();
            CShoppingCartItem item = new CShoppingCartItem();
            item.price = (decimal)prod.Price;
            item.productId = vm.txtProductID;
            item.count = vm.txtCount;
            item.product = prod;
            cart.Add(item);
            json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CDictionary.SK_PURCAHSED_PRODUCTS_LIST, json);

            return RedirectToAction("List");
        }
        public IActionResult List(string classification, string txtKeyword)
        {
            var data = db.Products.AsQueryable();

            // Get all unique classifications
            var allClassifications = db.Products.Select(p => p.Classification).Distinct().ToList();

            if (!string.IsNullOrEmpty(classification))
            {
                data = data.Where(p => p.Classification == classification);
            }

            if (!string.IsNullOrEmpty(txtKeyword))
            {
                data = data.Where(p => p.ProductName.Contains(txtKeyword));
            }

            // Prepare the list of CProductWrap
            List<CProductWrap> list = new List<CProductWrap>();
            foreach (var c in data)
            {
                CProductWrap w = new CProductWrap();
                w.product = c;
                list.Add(w);
            }

            // Create a new instance of the ProductListViewModel
            var viewModel = new ProductListViewModel
            {
                Products = list,
                AllClassifications = allClassifications
            };

            return View(viewModel);
        }




    }
}
