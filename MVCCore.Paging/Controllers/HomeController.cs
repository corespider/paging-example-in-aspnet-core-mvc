using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCCore.Paging.Context;
using MVCCore.Paging.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCore.Paging.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DatabaseContext Context { get; }
        public HomeController(ILogger<HomeController> logger, DatabaseContext _context)
        {
            _logger = logger;
            this.Context = _context;
        }

        public IActionResult Index()
        {
            return View(this.GetCustomers(1));
        }
        [HttpPost]
        public IActionResult Index(int currentPageIndex)
        {
            return View(this.GetCustomers(currentPageIndex));
        }

        private CustomerModel GetCustomers(int currentPage)
        {
            int maxRows = 10;
            var customers = this.Context.customers.Take(200).ToList();

            CustomerModel customerModel = new CustomerModel();
            customerModel.Customers = customers.OrderBy(customer => customer.customer_id)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();

            double pageCount = (double)((decimal)customers.Count() / Convert.ToDecimal(maxRows));
            customerModel.PageCount = (int)Math.Ceiling(pageCount);

            customerModel.CurrentPageIndex = currentPage;

            return customerModel;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
