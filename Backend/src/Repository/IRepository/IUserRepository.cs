using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Models;

namespace src.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User?> FindUserById(int id);
    }
}