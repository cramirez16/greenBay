using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Src.Models
{
    public class Item
    {
        // Using data annotations to configure a model / can be override by core fluent api in dbContext.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string PhotoUrl { get; set; }

        [Required]
        public required decimal Price { get; set; }

        public decimal Bid { get; set; } = 0m;
        [Required]
        public required bool IsSellable { get; set; } = true;

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [Required]
        public int SellerId { get; set; }

        public int? BuyerId { get; set; }

        // navigation property
        [ForeignKey("SellerId")]
        [Required]
        public User? Seller { get; set; }

        // navigation property
        [ForeignKey("BuyerId")]
        public User? Buyer { get; set; }

        // navigation property
        public List<Bid>? Bids { get; set; }

    }
}