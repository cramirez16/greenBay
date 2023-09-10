using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models.Specifications
{
    public class Parameters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } // How many records per page.
    }
}