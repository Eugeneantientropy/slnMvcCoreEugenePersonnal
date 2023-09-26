using Microsoft.AspNetCore.Authentication;

namespace prjMvcCoreEugenePersonnal.Models
{
    public class CProductWrap
    {
        private Product _product;
       public Product product
        {
            get { return _product; }
            set { _product = value; }
        }
        public CProductWrap()
        {
            _product = new Product();
        }
        public int ProductId
        {
            get { return _product.ProductId; }
            set { _product.ProductId = value; }
        }

        public string ProductName
        {
            get { return _product.ProductName; }
            set { _product.ProductName = value; }
        } 



        public string? Description
        {
            get { return _product.Description; }
            set { _product.Description = value; }
        }

        public decimal Price
        {
            get { return _product.Price; }
            set { _product.Price = value; }
        }

        public int StockQuantity
        {
            get { return _product.StockQuantity; }
            set { _product.StockQuantity = value; }
        }

        public DateTime? DateAdded
        {
            get { return _product.DateAdded; }
            set { _product.DateAdded = value; }
        }

        public string? ProductImagePath
        {
            get { return _product.ProductImagePath; }
            set { _product.ProductImagePath = value; }
        }
        public string Classification
        {
            get { return _product.Classification; }
            set { _product.Classification = value; }
        }
        public List<IFormFile> photos { get; set; }
        public IFormFile Photo { get; set; }


    }
}
