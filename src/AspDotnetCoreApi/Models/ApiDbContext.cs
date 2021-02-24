using Microsoft.EntityFrameworkCore;

namespace AspDotnetCoreApi.Models {
    public class ApiDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) {}
    }
}