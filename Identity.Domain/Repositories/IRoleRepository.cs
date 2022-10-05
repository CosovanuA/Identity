using Identity.Domain.Models;
using Identity.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Repositories
{
    public interface IRoleRepository : IRepository
    {
        Role AddRole(Role role);
        void RemoveRole(int id);
        Task<Role> GetRoleAsync(int id);
        Task<IList<Role>> GetRolesAsync(string[] roles);
    }
}
