using IdentityServerApp.AuthServer.Models;
using System.Threading.Tasks;

namespace IdentityServerApp.AuthServer.Repository
{
    public interface ICustomUserRepository
    {
        Task<bool> Validate(string email, string pass);
        Task<CustomUser> FindById(int id);
        Task<CustomUser> FindByEmail(string email);
    }
}
