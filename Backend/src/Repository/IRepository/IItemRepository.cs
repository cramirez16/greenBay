using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Models;

namespace src.Repository.IRepository
{
    public interface IItemRepository
    {
        Task<List<Item>> GetItemsAsync();
    }
}