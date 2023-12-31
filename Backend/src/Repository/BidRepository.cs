using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Src.Models;
using Src.Repository.IRepository;
using Src.Data;
using Microsoft.EntityFrameworkCore;

namespace Src.Repository
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
                    ItemId = bid.ItemId,
                    Item = bid.Item,
                    Bider = bid.Bider
                })
                .FirstOrDefaultAsync();
        }
        public async Task AddBidAsync(Bid newBid)
        {
            await _context.TblBids.AddAsync(newBid);
            //await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<int, string>> GetBidderNamesAsync(List<int> bidderIds)
        {
            return await _context.TblUsers
                            .Where(user => bidderIds.Contains(user.Id))
                            .ToDictionaryAsync(user => user.Id, user => user.Name);
        }

        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Modified;
            return;
        }

    }
}