using Microsoft.Identity.Client;
using prjMvcCoreEugenePersonnal.Models;

namespace prjMvcCoreEugenePersonnal.ViewModels
{
    public class CPostsViewModel
    {
        public Post Post { get; set; }
        public Post PrePost { get; set; }   
        public Post NextPost { get; set; }
    }
}
