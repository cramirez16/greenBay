using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Item
    {
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
        public bool IsSellable { get; set; } = true;

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [Required]
        public int SellerId { get; set; }

        [ForeignKey("SellerId")]
        public User? Seller { get; set; }

        public List<Bid>? Bids { get; set; }
    }
}