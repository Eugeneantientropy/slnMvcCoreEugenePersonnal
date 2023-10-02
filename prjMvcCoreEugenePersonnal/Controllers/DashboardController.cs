using Microsoft.AspNetCore.Mvc;
using prjMvcCoreEugenePersonnal.Models;

namespace prjMvcCoreEugenePersonnal.Controllers
{
    public class DashboardController : Controller
    {
        private readonly EugenePower0916Context _context;
        public DashboardController(EugenePower0916Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public List<object> GetOrder()
        {
            List<object> data = new List<object>();
            int currentYear = DateTime.Now.Year;

            List<string> labels = new List<string>();
            List<int> total = new List<int>();

            for (int month = 1; month <= 12; month++)
            {
                DateTime startDate = new DateTime(currentYear, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                string monthLabel = startDate.ToString("MMMM");

                int monthlyTotal = _context.Orders
                    .Where(o => o.DateOrdered >= startDate && o.DateOrdered <= endDate)
                    .Select(t => (int)t.TotalPrice)
                    .Sum();

                labels.Add(monthLabel);
                total.Add(monthlyTotal);
            }

            data.Add(labels);
            data.Add(total);
            return data;


        }
        [HttpPost]
        public List<object> NewMember()
        {
            List<object> data = new List<object>();
            int currentYear = DateTime.Now.Year;

            List<string> labels = new List<string>();
            List<int> total = new List<int>();

            for (int month = 1; month <= 12; month++)
            {
                DateTime startDate = new DateTime(currentYear, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                string monthLabel = startDate.ToString("MMMM");
                int monthlyUnshippedCount = _context.Users
                    .Where(m => m.DateRegistered >= startDate && m.DateRegistered <= endDate )
                    .Count();


                labels.Add(monthLabel);
                total.Add(monthlyUnshippedCount);
            }

            data.Add(labels);
            data.Add(total);
            return data;
        }
        [HttpPost]
        public List<object> ProductClassification()
        {
            List<object> data = new List<object>();

            List<string> labels = new List<string>();
            List<int> total = new List<int>();

            // 查询数据库以获取分类项和其数量
            var classificationCounts = _context.Products
                .GroupBy(p => p.Classification) // 假设分类项在数据表中的列名为 "Classification"
                .Select(group => new
                {
                    Classification = group.Key,
                    Count = group.Count()
                })
                .ToList();

            foreach (var classificationCount in classificationCounts)
            {
                labels.Add(classificationCount.Classification);
                total.Add(classificationCount.Count);
            }

            data.Add(labels);
            data.Add(total);

            return data;
        }

        [HttpPost]
        public List<object> unShiped()
        {
            List<object> data = new List<object>();
            int currentYear = DateTime.Now.Year;

            List<string> labels = new List<string>();
            List<int> total = new List<int>();

            for (int month = 1; month <= 12; month++)
            {
                DateTime startDate = new DateTime(currentYear, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                string monthLabel = startDate.ToString("MMMM");
                int monthlyUnshippedCount = _context.Orders
                    .Where(o => o.DateOrdered >= startDate && o.DateOrdered <= endDate && o.OrderStatus == "unshipped")
                    .Count();


                labels.Add(monthLabel);
                total.Add(monthlyUnshippedCount);
            }

            data.Add(labels);
            data.Add(total);    
            return data;
        }
    }
}
