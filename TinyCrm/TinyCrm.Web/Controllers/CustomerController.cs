using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TinyCrm.Core.Data;
using TinyCrm.Core.Model;
using TinyCrm.Core.Services;
using TinyCrm.Core.Services.Options;

namespace TinyCrm.Web.Controllers
{
    public class CustomerController : Controller
    {
        private TinyCrmDbContext dbContext_;
        private ICustomerService customerService_;
        public CustomerController()
        {
            dbContext_ = new TinyCrmDbContext();
            customerService_ = new CustomerService(dbContext_);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerOptions options)
        {
            var result = customerService_.CreateCustomer(options);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }

            return Json(result.Data);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var customerList = customerService_
                .SearchCustomers(new SearchCustomerOptions())
                .ToList();

            return Json(customerList);
        }

        // http://localhost/customer/5
        // GET: retrieve a customer's info
        [HttpGet("{id}")]
        public IActionResult Details(int? id)
        {
            var customer = customerService_.GetCustomerById(id).Data;

            return View(customer);
        }

        // http://localhost/customer/{id}/edit
        [HttpGet("{id}/edit")]
        public IActionResult Edit(int id)
        {
            var customer = customerService_.GetCustomerById(id).Data;

            return View(customer);
        }

        // http://localhost/customer/5
        // PATCH: update a customers info
        [HttpPatch("{id}")]
        public IActionResult UpdateCustomer(int id,
            [FromBody] UpdateCustomerOptions options)
        {
            var result = customerService_.UpdateCustomer(id,
                options);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }

            return Ok();
        }        
    }
}