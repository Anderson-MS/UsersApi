using Microsoft.EntityFrameworkCore;
using Users.Api.Models;

namespace Users.Api.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
             
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
