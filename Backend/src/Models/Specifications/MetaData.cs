using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models.Specifications
{
    public class MetaData
    {
        public int TotalPages { get; set; } // How many Pages.
        public int PageSize { get; set; }  // Records per page.
        public int TotalCount { get; set; } // Total number of records
    }
}