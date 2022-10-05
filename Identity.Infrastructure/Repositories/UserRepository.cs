using Identity.Domain.Repositories;
using Identity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Identity.Domain.Shared;
using System.Security.Cryptography;

namespace Identity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityContext _Context;

        public UserRepository(IdentityContext context)
        {
            _Context = context;
        }

        public IUnitOfWork UnitOfWork => _Context;

        public User AddUser(string username, string password, string email)
        {
            User user = new() { UserName = username, Email = email };

            return _Context.Users.Add(user).Entity;
        }

        public async Task<bool> LoginUser(string username, string password)
        {
            var existingUser = await GetUserByUsernameAsync(username);

            if(existingUser != null)
            {
                return true;
            }

            return false;
        }

        public void RemoveUser(int id)
        {
            User user = _Context.Users.Find(id);
            _Context.Users.Remove(user);
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            return await _Context.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _Context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _Context.Users.Where(u => u.UserName.ToLower() == username.ToLower()).FirstOrDefaultAsync();
        }

        private bool _EqualByteArrays(byte[] first, byte[] second)
        {
            if(first.Length != second.Length) { return false; }

            for(int i = 0; i < first.Length; i++)
            {
                if(first[i] != second[i]) { return false; }
            }

            return true;
        }
    }
}
