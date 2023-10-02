using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;
using prjMvcCoreEugenePersonnal.ViewModels;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class PostManagerController : ManagerController
    {
        EugenePower0916Context db = new EugenePower0916Context();
        private IWebHostEnvironment _envior = null;
        public PostManagerController(IWebHostEnvironment p)
        {
            _envior = p;
        }
        public IActionResult List(CKeywordViewModel vm)
        {
            IEnumerable<Post> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.Posts
                        select p;
            else
                datas = db.Posts.Where(p => p.Title.Contains(vm.txtKeyword));
            return View(datas);

        }
        public async Task<IActionResult> Delete(int id)
        {
            var post = await db.Posts.FindAsync(id);
            if(post == null) 
            {
                return NotFound();
            }
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(List));

        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            Post p = db.Posts.FirstOrDefault(p =>p.PostId == id);

            if (p == null)
                return RedirectToAction("List");
            CPostWrap postWP = new CPostWrap();
            postWP.post = p;
            return View(postWP);

        }
        [HttpPost]
        public IActionResult Edit(CPostWrap post)
        {
            Post p = db.Posts.FirstOrDefault(p=>p.PostId == post.PostId);
            if (post.Photo != null)
            {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string path = _envior.WebRootPath + "/images/Post/" + photoName;
                post.Photo.CopyTo(new FileStream(path, FileMode.Create));
                post.PostImageUrl = photoName;
            }
            p.Title = post.Title;
            p.Content = post.Content;   
            p.LastEdited = DateTime.Now;
            p.PostImageUrl = post.PostImageUrl; 

            db.SaveChanges();   

            return RedirectToAction("List");  
        }


    }
}
