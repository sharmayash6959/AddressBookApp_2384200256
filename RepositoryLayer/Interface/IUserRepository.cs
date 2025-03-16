using ModelLayer.Model;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<bool> CreateUser(User user);
    }
}
