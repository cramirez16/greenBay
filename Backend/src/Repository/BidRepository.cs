using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Models;
using src.Repository.IRepository;
using src.Data;
using Microsoft.EntityFrameworkCore;

namespace src.Repository
{
    public class BidRepository : IBidRepository
    {
        private readonly GreenBayDbContext _context;

        public BidRepository(GreenBayDbContext context)
        {
            _context = context;
        }

        public async Task<Bid?> GetMaxBidByItemId(int itemId)
        {
            return await _context.TblBids
                .Where(bid => bid.ItemId == itemId)
                .OrderByDescending(bid => bid.BidAmount)
                .Select(bid => new Bid
                {
                    Id = bid.Id,
                    BidAmount = bid.BidAmount,
                    BiderId = bid.BiderId,
                    ItemId = bid.ItemId
                })
                .FirstOrDefaultAsync();
        }
        public async Task AddBidAsync(Bid newBid)
        {
            _context.TblBids.Add(newBid);
            await _context.SaveChangesAsync();
        }
    }
}