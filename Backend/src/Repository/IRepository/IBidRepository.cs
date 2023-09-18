using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Models;

namespace src.Repository.IRepository
{
    public interface IBidRepository
    {
        Task<Bid?> GetMaxBidByItemId(int itemId);
        Task AddBidAsync(Bid newBid);
        Task<Dictionary<int, string>> GetBidderNamesAsync(List<int> bidderIds);
        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : class;
    }
}