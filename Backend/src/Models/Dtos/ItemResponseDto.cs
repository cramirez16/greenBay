using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Src.Models.Dtos
{
    public class ItemResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        // public required string Description { get; set; }
        public required string Description { get; set; }
        public required string PhotoUrl { get; set; }
        // public required decimal Price { get; set; }
        public decimal Bid { get; set; } = 0m;
        public required decimal Price { get; set; }
        public bool IsSellable { get; set; } = true;
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int SellerId { get; set; }
        public string? SellerName { get; set; }
        public int BuyerId { get; set; }
        public string? BuyerName { get; set; }
        public List<BidResponseDto>? Bids { get; set; }
    }
}