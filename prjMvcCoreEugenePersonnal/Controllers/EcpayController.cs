using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using prjMvcCoreEugenePersonnal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class EcpayController : Controller
    {
        private readonly EugenePower0916Context _context;
        private readonly IMemoryCache _memoryCache;
        public EcpayController(EugenePower0916Context context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public IActionResult Index(string id)
        {
            var order = _context.Orders.FirstOrDefault(x => x.EcpayId==id);
            if(order == null)
            {
                return NotFound("Order not found!");
            }
            //放訂單所有商品
            string orderProducts = "";

            //放本站網址,訂單編號,價錢,商品名稱
            var website = $"https://c528-140-116-180-210.ngrok-free.app";
            //var website = $"https://localhost:7079";
            var orderId = id;
            var orderPrice = (int)order.TotalPrice;
            var orderItems = _context.OrderItems
                                 .Include(oi => oi.Product) // Assuming the Product navigation property is set up correctly
                                 .Where(oi => oi.OrderId == order.OrderId)
                                 .ToList();

            foreach (var item in orderItems)
            {
                orderProducts += item.Product.ProductName + "| "; // Assuming the ProductName property exists on the Product entity
            }

            var orderCheck = new Dictionary<string, string>
        {
            { "MerchantID",  "3002607"},
            { "MerchantTradeNo",  id},
            { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
            { "PaymentType",  "aio"},
            { "TotalAmount",  orderPrice.ToString()},
            { "TradeDesc",  "無"},
            { "ItemName",  orderProducts},
            { "ReturnURL",  $"{website}/api/EcpayApi/AddPayInfo"},
            { "ChoosePayment",  "ALL"},
            { "ClientBackURL",  $"{website}/Ecpay/AccountInfo/{orderId}"},
            { "OrderResultURL", $"{website}/Ecpay/PayInfo/{orderId}"},
            { "IgnorePayment",  "WebATM#ATM#CVS#BARCODE#TWQR"},
            { "EncryptType",  "1"}
        };

            orderCheck["CheckMacValue"] = GetCheckMacValue(orderCheck);

            return View(orderCheck);

        }

        [HttpPost]
        public IActionResult PayInfo(string id, IFormCollection data)
        {
            EugenePower0916Context db = new EugenePower0916Context();
            var req = HttpContext.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

            string temp = data["MerchantTradeNo"]; //寫在LINQ會出錯
            var order = db.Orders.FirstOrDefault(x => x.EcpayId == temp);
            if (order != null)
            {
                int orderID = order.OrderId;
                req.Add("ActivityID", orderID.ToString());
            }

                var cacheData = _memoryCache.Get(id);
            if (req != null)
            {

                return View(req);
            }
            else
            {
                // 在快取中找不到資料的處理邏輯
                return NotFound();
            }
        }

        private string GetCheckMacValue(Dictionary<string, string> orderCheck)
        {
            var param = orderCheck.Keys.OrderBy(x => x).Select(key => key + "=" + orderCheck[key]).ToList();
            var checkValue = string.Join("&", param);

            // 綠界金流提供參數，須放在checkValue前後
            var hashKey = "pwFHCqoQZGmho4w6";
            var HashIV = "EkRm7iFT261dpevs";

            checkValue = $"HashKey={hashKey}&{checkValue}&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            return GetSHA256(checkValue).ToUpper();
        }

        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        [HttpPost]
        public IActionResult AccountInfo(IFormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            EugenePower0916Context db = new EugenePower0916Context();
            string temp = id["MerchantTradeNo"]; //寫在LINQ會出錯
            var ecpayOrder = db.Orders.Where(m => m.EcpayId == temp).FirstOrDefault();

            //商城
            var ecpayOrderProduct = db.Orders.Where(m => m.EcpayId == temp).FirstOrDefault();

            if (ecpayOrder != null)
            {
                //ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
                //if (id["RtnMsg"] == "Succeeded") ecpayOrder.RtnMsg = "已付款";
                //ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
                //ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
                //db.SaveChanges();
            }
 

            return View(data);
        }
    }
}
