using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Src.Models.Dtos
{
    public class BidRequestDto
    {
        public required int BiderId { get; set; }
        public required decimal BidAmount { get; set; }
        public required int ItemId { get; set; }
    }
}