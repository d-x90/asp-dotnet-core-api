using System.Collections.Generic;
using System.Threading.Tasks;
using AspDotnetCoreApi.Models;

namespace AspDotnetCoreApi.Repositories {
    public interface IUserRepository {

        Task<User> GetCurrentUserAsync();
        Task<List<User>> GetAllUsersAsync();
        
        Task<User> GetUserByIdAsync(int id);

        Task<int> SaveChangesAsync();

        void RemoveUser(User user);
    }
}