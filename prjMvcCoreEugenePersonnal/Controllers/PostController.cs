using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using OpenAI_API.Completions;
using OpenAI_API;


namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class PostController : Controller
    {
        private readonly EugenePower0916Context _context;
        public PostController(EugenePower0916Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.OrderByDescending(p=>p.DatePosted).Take(6).ToListAsync());
        }
        public IActionResult Create()
        {
            if (CLogginUsercs.LoggedUser != "employee")
            {
                CAlertMessage result = new CAlertMessage(CDictionary.Danger, "僅限管理員權限進入");
                TempData["message"] = JsonSerializer.Serialize<CAlertMessage>(result);
                return RedirectToAction("Index");

            }
            var vm = new CPostCreateViewModel
            {
                DatePosted = DateTime.Now,
                LastEdited = DateTime.Now,
            };
            return View(vm);
        }

            [HttpPost]
            public async Task<IActionResult> Create(CPostCreateViewModel vm)
            {
                if (vm.ImageFile != null && vm.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(vm.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/post", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        vm.ImageFile.CopyTo(stream);
                    }

                    vm.PostImageUrl = fileName;
                    ModelState.Remove("PostImageURL");
                }

                if (ModelState.IsValid)
                {
                    Post newsItem = new Post
                    {
                        Title = vm.Title,
                        Content = vm.Content,
                        PostImageUrl = vm.PostImageUrl,
                        LastEdited = DateTime.Now,
                        DatePosted = DateTime.Now,
                        UserId = 1, 
                    };
                    _context.Posts.Add(newsItem);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    ViewBag.Errors = errors;

                    return View(vm);
                }
            }

        [HttpPost]
        public async Task<IActionResult> GetSuggestion(string content)
        {
      
                string APIKey = CApiKey.AIApiKey;
                string answer = string.Empty;
                var openai = new OpenAIAPI(APIKey);

                CompletionRequest completion = new CompletionRequest();
                completion.Prompt = content;
                completion.Model = OpenAI_API.Models.Model.DavinciText;
                completion.MaxTokens = 500;

                var result = await openai.Completions.CreateCompletionAsync(completion);

                var suggestion = result.Completions[0].Text;

                return Json(new { suggestion });
        }


        public class OpenAIResponse
            {
                public class Choice
                {
                    public string Text { get; set; }
                }

                public List<Choice> Choices { get; set; }
            }
 

        public async Task<IActionResult> PostDetail(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            var prePost = await _context.Posts.Where(p => p.PostId < id).OrderByDescending(p => p.PostId).FirstOrDefaultAsync();
            var nextPost = await _context.Posts.Where(p => p.PostId > id).OrderBy(p => p.PostId).FirstOrDefaultAsync();

            var viewModel = new CPostsViewModel
            {
                Post = post,
                PrePost = prePost,
                NextPost = nextPost
            };
            ViewBag.UrlToShare = Url.Action("PostDetail", "Post", new { id = id }, Request.Scheme);
            return View(viewModel);
        }


    }
}
