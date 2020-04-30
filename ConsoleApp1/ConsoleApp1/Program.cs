using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = @"C:\Users\goodm\devel\TinyCRMGit\ConsoleApp1\ConsoleApp1\products.csv";
            string[] lines = File.ReadAllLines(path);
            decimal price = 0M;            
            lines = lines.Skip(1).ToArray();
            List<Product> ProductList = new List<Product>();
            foreach(string line in lines)
            {
                var values = line.Split(';');
                price = GetRandomDecimal();
                if (!(ProductList.Any(item => item.ProductID == values[0]))) ;
                {
                    ProductList.Add(new Product(values[0], values[1], values[2], price));
                }                
            }
            Console.WriteLine("Done");
        } 
        
        public static decimal GetRandomDecimal()
        {
            var random = new Random();
            var randomNumber = random.NextDouble() * 100;
            return (decimal)Math.Round(randomNumber, 2);
            
        }
    }
}