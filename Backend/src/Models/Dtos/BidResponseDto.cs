using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models.Dtos
{
    public class BidResponseDto
    {
        public int Id { get; set; }
        public decimal BidAmount { get; set; }
        public int BiderId { get; set; }
        public string? BiderName { get; set; }
        public int ItemId { get; set; }
    }
}