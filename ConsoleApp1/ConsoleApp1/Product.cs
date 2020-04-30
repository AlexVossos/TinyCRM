using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Product
    {
        public string ProductID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string productID, string name, string description, decimal price)
        {
            this.ProductID = productID;
            this.Name = name;
            this.Description = description;
            this.Price = price;
        }
    }
}
