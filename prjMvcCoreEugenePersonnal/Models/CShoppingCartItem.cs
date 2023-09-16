using System.ComponentModel;

namespace prjMvcCoreEugenePersonnal.Models
{
    public class CShoppingCartItem
    {
        public int productQTY { get; set; }
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
        public List<CShoppingCartItem> ShoppingCartItems { get; set; }//
        public int UserID { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal PayableAmount { get; set; }
    }
}
