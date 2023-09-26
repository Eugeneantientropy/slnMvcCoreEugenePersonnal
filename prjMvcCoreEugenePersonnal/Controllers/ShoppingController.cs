using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;
using System.Text.Json;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class ShoppingController : SuperController
    {
        EugenePower0916Context db = new EugenePower0916Context();
        private IWebHostEnvironment _envior = null;

        public ShoppingController(IWebHostEnvironment envior)
        {
            _envior = envior;
        }

        public IActionResult CartView()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_PURCAHSED_PRODUCTS_LIST))
            {
                CAlertMessage resultb = new CAlertMessage(CDictionary.Warning, "尚無商品在購物車");
                TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(resultb);
                return RedirectToAction("List");
            }

            string json = HttpContext.Session.GetString(CDictionary.SK_PURCAHSED_PRODUCTS_LIST);
            List<CShoppingCartItem> cart = JsonSerializer.Deserialize<List<CShoppingCartItem>>(json)
                as List<CShoppingCartItem>;

            string userJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            User user = null;
            if (userJson != null)
            {
                user = JsonSerializer.Deserialize<User>(userJson);
            }

            if (cart == null)
                return RedirectToAction("List");

            ViewBag.User = user;
            return View(cart);
        }

        public IActionResult AddToCart(int? id)
        {
            List<ProductPhoto> prodimgList = db.ProductPhotos.Where(t => t.ProductId == id).ToList();
            ViewBag.ProdImgList = prodimgList;
            if (id == null)
            {
                return RedirectToAction("List");
            }

            Product prod = db.Products.FirstOrDefault(t => t.ProductId == id);
            if (prod == null)
            {
                return RedirectToAction("List");
            }
            ViewBag.Product = prod;
            return View();
        }


        [HttpPost]
        public IActionResult AddToCart(CAddToCartViewModel vm)
        {
            // 將圖片列表添加到 ViewBag

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
            item.productQTY = prod.StockQuantity;

            cart.Add(item);
            json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CDictionary.SK_PURCAHSED_PRODUCTS_LIST, json);

            return RedirectToAction("List");
        }

        public IActionResult CartCheckout(CShoppingCartItem vm)
        {
            var currentTime = DateTime.Now;//現在時間
            var ecpayId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);//產生20碼專屬變數給ecpayId

            Order o = new Order();

            o.TotalPrice = (int)vm.TotalAmount;
            o.UserId = vm.UserID;
            o.DateOrdered = currentTime;
            o.ShippingAddress = vm.Address;
            o.OrderStatus = "未出貨";
            o.EcpayId = ecpayId;

            db.Orders.Add(o);
            db.SaveChanges();

            string json = HttpContext.Session.GetString(CDictionary.SK_PURCAHSED_PRODUCTS_LIST);

            if(json != null)
            {
                List<CShoppingCartItem> cart = JsonSerializer.Deserialize<List<CShoppingCartItem>>(json);
                int orderId = o.OrderId;

                foreach(var item in cart)
                {
                    int pID = item.productId;
                    int price = (int)item.price;
                    int qty = item.count;
                    //放進OrderItem資料表
                    OrderItem OI = new OrderItem(); 
                    OI.OrderId = orderId;
                    OI.ProductId = pID;
                    OI.PriceAtPurchase = price;
                    OI.Quantity = qty;

                    db.OrderItems.Add(OI);

                    //庫存減少
                    var newStockItem = db.Products.FirstOrDefault(p=> p.ProductId == pID);  
                    if(newStockItem != null)
                    {
                        newStockItem.StockQuantity = newStockItem.StockQuantity - qty;
                    }
                    db.SaveChanges();
                    HttpContext.Session.Remove(CDictionary.SK_PURCAHSED_PRODUCTS_LIST);

                }
            }
            TempData["EcpayId"] = ecpayId;
            return RedirectToAction("RegisterJump", new { id = ecpayId });
        }

        public IActionResult RegisterJump()//結帳跳轉頁面
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterJump(string id)//接RegisterActivity的ecpayId
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID cannot be null or empty" });
            }
            TempData["EcpayId"] = id;
            return Json(new { success = true });
        }
        //}
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
