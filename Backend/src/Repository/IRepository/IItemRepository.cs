using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Src.Models;
using Src.Models.Specifications;

namespace Src.Repository.IRepository
{
    public interface IItemRepository
    {
        Task<List<Item>> GetItemsAsync();
        Task<Item?> GetItemByIdAsync(int id);
        Task<Item?> FindItemByIdAsync(int id);
        Task SaveItemAsync(Item item);
        Task<PagedList<Item>> GetItemsPaginatedAsync(Parameters parameters);
    }
}