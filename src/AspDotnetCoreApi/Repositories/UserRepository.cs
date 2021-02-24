using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspDotnetCoreApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AspDotnetCoreApi.Repositories {
    public class UserRepository : IUserRepository
    {
        private readonly ApiDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public UserRepository(ApiDbContext context, IHttpContextAccessor httpContext) {
            _context = context;
            _httpContext = httpContext;
        } 

        public Task<List<User>> GetAllUsersAsync()
        {
            return _context.Users.ToListAsync();
        }

        public Task<User> GetCurrentUserAsync()
        {
            var userId = int.Parse(_httpContext.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
   
            return _context.Users.FindAsync(userId).AsTask();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void RemoveUser(User user)
        {
            _context.Users.Remove(user);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}