using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;
using System.Text.Json;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class ShoppingController : SuperController
    {

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
        public IActionResult List() { 
            EugenePowerContext db = new EugenePowerContext();  
            var data = from p in db.Products select p;
            List<CProductWrap> list = new List<CProductWrap>();
            foreach (var c in data) {
                CProductWrap w = new CProductWrap();
                w.product = c;
                list.Add(w);   
            }
            return View(list);
        }

    }
}
