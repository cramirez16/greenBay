using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Bid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public decimal BidAmount { get; set; }
        [Required]
        public int BidderId { get; set; }
        [ForeignKey("BidderId")]
        public User? User { get; set; }
        [Required]
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public required Item Item { get; set; }

        // Navigation property for the UserBid junction table
        // public List<UserBid> UserBids { get; set; }
    }
}