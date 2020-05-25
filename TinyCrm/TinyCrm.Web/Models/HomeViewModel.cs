using System;
using System.Collections.Generic;
using TinyCrm.Core.Model;

namespace TinyCrm.Web.Models
{
    public class HomeViewModel
    {
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }

        public HomeViewModel()
        {
            Customers = new List<Customer>();
            Products = new List<Product>();
        }
    }
}