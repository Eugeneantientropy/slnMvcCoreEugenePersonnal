using Microsoft.EntityFrameworkCore;

namespace prjMvcCoreEugenePersonnal.Models
{
    public class CUserWrap
    {
        public List<OrderItem> OrderItem { get; set; } = new List<OrderItem>();

        private User _user;


        public class OrderDetailInfo
        {
            public int ProductQty { get; set; }
            public int ProductUnitPrice { get; set; }
            public string ProductName { get; set; }
        }
        public User user
        {
            get { return _user; }
            set
            {
                _user = value;
                EugenePower0916Context db = new EugenePower0916Context();


                OrderCount = db.Orders.Count(od => od.UserId
                == value.UserId);

            }
        }

        public CUserWrap()
        {
            _user = new User();

        }


        public int OrderCount { get; set; }
        public int UserId
        {
            get { return _user.UserId; }
            set { _user.UserId = value; }
        }

        public string? Username
        {
            get { return _user.Username; }
            set { _user.Username = value; }
        }

        public string? FullName
        {
            get { return _user.FullName; }
            set { _user.FullName = value; }
        }

        public string? Address
        {
            get { return _user.Address; }
            set { _user.Address = value; }
        }

        public string? Phone
        {
            get { return _user.Phone; }
            set { _user.Phone = value; }
        }

        public string Email
        {
            get { return _user.Email; }
            set { _user.Email = value; }
        }

        // OrderItem 相關屬性
        public virtual ICollection<OrderItem> OrderDetail1s { get; set; } = new List<OrderItem>();

        // Order 相關屬性
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

