using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Src.Models.Specifications
{
    public class QueryParameters
    {
        public required int Id { get; set; }
        public required string Password { get; set; }
    }
}