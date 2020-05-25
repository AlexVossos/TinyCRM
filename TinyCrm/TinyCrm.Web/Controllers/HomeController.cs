using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyCrm.Web.Models;
using TinyCrm.Core.Data;
using TinyCrm.Core.Migrations;
using TinyCrm.Core.Model;
using TinyCrm.Core.Services;
using TinyCrm.Core.Services.Options;

namespace TinyCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        private CustomerService customerService_;
        private TinyCrmDbContext context;

        public HomeController()
        {
            context = new TinyCrmDbContext();
            customerService_ = new CustomerService(
               context);
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel()
            {
                Customers = customerService_.SearchCustomers(
                        new SearchCustomerOptions())
                    .ToList(),
                Products = context.Set<Product>()
                    .ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            using (var context = new TinyCrmDbContext())
            {
                var customerService = new CustomerService(context);
            }
               
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
