using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class UserBid
    {
        // [Key]
        // public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; } // Navigation property to User entity
        public int BidId { get; set; }
        public Bid? Bid { get; set; } // Navigation property to Bid entity
    }
}