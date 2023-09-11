using System.ComponentModel;

namespace prjMvcCoreEugenePersonnal.Models
{
    public class CShoppingCartItem
    {
        public int productId { get; set; }
        [DisplayName("採購數量")]
        public int count { get; set; }
        [DisplayName("金額")]
        public decimal price { get; set; }
        public decimal 小計
        {
            get { return count * price; }
        }
        public Product product { get; set; }
    }
}
