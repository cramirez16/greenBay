using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Src.Models;

namespace Src.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User?> FindUserById(int id);
        Task<User?> FindUserByEmail(string email);
        Task AddUser(User newUser);
        Task SaveToDbAsync();
        Task<IEnumerable<User>> GetUsers();
        void SaveToDb();
        void DeleteUser(User user);
    }
}