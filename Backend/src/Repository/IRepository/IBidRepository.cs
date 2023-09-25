using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Src.Models;

namespace Src.Repository.IRepository
{
    public interface IBidRepository
    {
        Task<Bid?> GetMaxBidByItemId(int itemId);
        Task AddBidAsync(Bid newBid);
        Task<Dictionary<int, string>> GetBidderNamesAsync(List<int> bidderIds);
        Task SaveChangesAsync();
        void UpdateEntity<TEntity>(TEntity entity) where TEntity : class;
    }
}