using Identity.Domain.Models;
using Identity.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Repositories
{
    public interface IUserRepository : IRepository
    {
        User AddUser(string username, string password, string email);
        void RemoveUser(int id);
        Task<User> GetUserAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<IList<User>> GetUsersAsync();
        Task<bool> LoginUser(string username, string password);
    }
}
