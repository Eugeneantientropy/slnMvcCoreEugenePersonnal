using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using prjMvcCoreEugenePersonnal.Models;
using OpenAI_API.Completions;
using OpenAI_API;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class AIController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> GetABaseResult(string SearchText)      
        {
            string APIKey = "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\r\n";
            string answer = string.Empty;
            var openai = new OpenAIAPI(APIKey);

            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = SearchText;
            completion.Model = OpenAI_API.Models.Model.DavinciText;
            completion.MaxTokens = 200;

            var result = openai.Completions.CreateCompletionAsync(completion);


            foreach(var item in result.Result.Completions)
            {
                answer = item.Text;
            }
            return Ok(answer);
        }

    }
}
