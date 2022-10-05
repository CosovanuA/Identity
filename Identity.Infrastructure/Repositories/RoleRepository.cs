using Identity.Domain.Models;
using Identity.Domain.Repositories;
using Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public IUnitOfWork UnitOfWork => _Context;
        private readonly IdentityContext _Context;


        public RoleRepository(IdentityContext context)
        {
            _Context = context;
        }


        public Role AddRole(Role role)
        {
            return _Context.Roles.Add(role).Entity;
        }

        public async Task<Role> GetRoleAsync(int id)
        {
            return await _Context.Roles.FindAsync(id);
        }

        public async Task<IList<Role>> GetRolesAsync(string[] roles)
        {
            return await _Context.Roles.ToListAsync();
        }

        public void RemoveRole(int id)
        {
            Role role = _Context.Roles.Find(id);
            _Context.Roles.Remove(role);
        }
    }
}
