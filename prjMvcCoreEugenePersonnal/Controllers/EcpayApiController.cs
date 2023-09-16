using prjMvcCoreEugenePersonnal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EcpayApiController : ControllerBase
    {

        private readonly IMemoryCache _memoryCache;

        public EcpayApiController(IMemoryCache memoryCache)
        {

            _memoryCache = memoryCache;
        }



        /// <summary>
        /// 綠界回傳 付款資訊
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public HttpResponseMessage AddPayInfo()
        {
            var req = HttpContext.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            Console.WriteLine(req.ToString());
            try
            {
                //_memoryCache.Set(info.Value<string>("MerchantTradeNo"), info, TimeSpan.FromMinutes(60));
                return ResponseOK();
            }
            catch (Exception e)
            {
                return ResponseError();
            }
        }

        /// <summary>
        /// 綠界回傳 虛擬帳號資訊
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddAccountInfo(JObject info)
        {
            Console.WriteLine(info.ToString());
            try
            {
                _memoryCache.Set(info.Value<string>("MerchantTradeNo"), info, TimeSpan.FromMinutes(60));
                return ResponseOK();
            }
            catch (Exception e)
            {
                return ResponseError();
            }
        }

        /// <summary>
        /// 回傳給 綠界 失敗
        /// </summary>
        /// <returns></returns>
        private HttpResponseMessage ResponseError()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("0|Error");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        /// <summary>
        /// 回傳給 綠界 成功
        /// </summary>
        /// <returns></returns>
        private HttpResponseMessage ResponseOK()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("1|OK");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

    }
}
