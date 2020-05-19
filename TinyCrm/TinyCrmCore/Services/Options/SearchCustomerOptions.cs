using System;

namespace TinyCrm.Core.Services.Options
{
    public class SearchCustomerOptions
    {
        public string Firstname { get; set; }
        public string VatNumber { get; set; }
        public int? CustomerId { get; set; }
        public DateTimeOffset? CreateFrom { get; set; }
    }
}
