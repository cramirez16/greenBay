using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Repository.IRepository;

namespace src.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly GreenBayDbContext _context;

        public ItemRepository(GreenBayDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            return await _context.TblItems
                .Include(item => item.Seller)
                .Include(item => item.Bids)
                .Where(item => item.IsSellable)
                .ToListAsync();
        }
    }
}