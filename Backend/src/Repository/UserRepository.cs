using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task<User?> FindUserByEmail(string email)
        {
            return await _context.TblUsers.FirstOrDefaultAsync(
                user => user.Email == email);
        }

        public async Task AddUser(User newUser)
        {
            await _context.TblUsers.AddAsync(newUser);
            return;
        }

        public async Task SaveToDbAsync()
        {
            await _context.SaveChangesAsync();
            return;
        }

        public void SaveToDb()
        {
            _context.SaveChanges();
            return;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.TblUsers.ToListAsync();
        }

        public void DeleteUser(User user)
        {
            _context.TblUsers.Remove(user);
            return;
        }

    }
}