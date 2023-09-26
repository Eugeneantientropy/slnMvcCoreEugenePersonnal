using System.Drawing;

namespace prjMvcCoreEugenePersonnal.ViewModels
{
    public class CPostCreateViewModel
    {
        public int PostId { get; set; }

        public int? UserId { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;
        public string PostImageUrl { get; set; }

        public IFormFile ImageFile { get; set; }


        public DateTime? DatePosted { get; set; }

        public DateTime? LastEdited { get; set; }
    }
}
