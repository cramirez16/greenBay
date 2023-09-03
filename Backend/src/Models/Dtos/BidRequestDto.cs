using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models.Dtos
{
    public class BidRequestDto
    {
        public required decimal BidAmount { get; set; }
        public required int BiderId { get; set; }
        public required int ItemId { get; set; }
    }
}