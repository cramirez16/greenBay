using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Data;
using src.Models;
using src.Repository.IRepository;

namespace src.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly GreenBayDbContext _context;

        public UserRepository(GreenBayDbContext context)
        {
            _context = context;
        }
        public async Task<User?> FindUserById(int id)
        {
            return await _context.TblUsers.FindAsync(id);
        }
    }
}