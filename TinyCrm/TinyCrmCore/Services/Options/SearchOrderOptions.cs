using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCrm.Core.Services.Options
{
    public class SearchOrderOptions
    {
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
        public List<string> ProductIds { get; set; }
    }
}
