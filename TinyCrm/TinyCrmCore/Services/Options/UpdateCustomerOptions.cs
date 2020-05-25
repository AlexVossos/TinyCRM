using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCrm.Core.Services.Options
{
    public class UpdateCustomerOptions
    {
        public string Vatnumber { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
    }
}
