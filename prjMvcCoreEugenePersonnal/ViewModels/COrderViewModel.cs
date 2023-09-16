using prjMvcCoreEugenePersonnal.Models;

namespace prjMvcCoreEugenePersonnal.ViewModels
{
    public class COrderViewModel
    {
        public List<CShoppingCartItem> ShoppingCartItems { get; set; }//
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal PayableAmount { get; set; }
    }
}
