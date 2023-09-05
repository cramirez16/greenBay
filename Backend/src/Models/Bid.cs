using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
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
        public int BiderId { get; set; }
        [ForeignKey("BiderId")]
        public User? Bider { get; set; }
        [Required]
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        [JsonIgnore]
        public Item? Item { get; set; }

        // Navigation property for the UserBid junction table
        public List<UserBid>? BidToUserBids { get; set; }
    }
}