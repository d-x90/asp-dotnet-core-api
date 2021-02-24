using System.Threading.Tasks;
using AspDotnetCoreApi.Models;

namespace AspDotnetCoreApi.Services {
    public interface IAuthenticationService {
        Task<int> Register(User user, string password);
        Task<string> Login(string usernameOrEmail, string password);

        Task<bool> IsUserPresent(string username, string email);
    }
}