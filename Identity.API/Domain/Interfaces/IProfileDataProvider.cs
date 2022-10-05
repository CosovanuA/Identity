using IdentityServer4.Models;
using System.Threading.Tasks;

namespace Identity.API.Domain.Interfaces
{
    public interface IProfileDataProvider
    {
        Task SetClaims(ProfileDataRequestContext context);
    }
}
