using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string VatNumber { get; set; }
        public string Phone { get; set; }
        public decimal TotalGross { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }

        public Customer()
        {

        }

        public bool IsValidVatNumber(string VatNumber)
        {
            if (!(string.IsNullOrWhiteSpace(VatNumber)))
            {
                long testNum;
                VatNumber = VatNumber.Trim();
                if (long.TryParse(VatNumber, out testNum) && VatNumber.Length == 9)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public bool IsAdult(int Age)
        {
            return Age >= 18 && Age < 200;
        }

        public bool IsValidEmail(string Email)
        {
            if (!(string.IsNullOrWhiteSpace(Email)))
            {
                Email = Email.Trim();
                if (Email.Contains("@") && Email.EndsWith(".com"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
    }
}