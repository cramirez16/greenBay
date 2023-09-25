using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Src.Models.Specifications
{
    public class Parameters
    {
        [Required]
        public int PageNumber { get; set; }
        [Required]
        public int PageSize { get; set; } // How many records per page.
    }
}