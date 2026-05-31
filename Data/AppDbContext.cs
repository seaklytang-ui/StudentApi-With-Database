using Microsoft.EntityFrameworkCore;
using StudentApi.Models;

namespace StudentApi.Data
{
    public class AppDbContext : DbContext
    {
        public  AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }
        public  DbSet<Student> Students { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
