using prjMvcCoreEugenePersonnal.Models;

namespace prjMvcCoreEugenePersonnal.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<CProductWrap> Products { get; set; }
        public IEnumerable<string> AllClassifications { get; set; }
    }
}
