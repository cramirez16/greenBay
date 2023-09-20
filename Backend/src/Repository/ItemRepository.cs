using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Models.Specifications;
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

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _context.TblItems
                            .Include(item => item.Seller)
                            .Include(item => item.Bids)
                            .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<Item?> FindItemByIdAsync(int id)
        {
            return await _context.TblItems.FindAsync(id);
        }

        public async Task SaveItemAsync(Item item)
        {
            await _context.TblItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedList<Item>> GetItemsPaginatedAsync(Parameters parameters)
        {
            var query = await _context.TblItems
               .Include(item => item.Seller)
               .Include(item => item.Bids)
               .Where(item => item.IsSellable)
               .ToListAsync();
            return PagedList<Item>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }
    }
}