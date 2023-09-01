using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class UserBid
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int BidId { get; set; }
        public Bid? Bid { get; set; }
    }
}