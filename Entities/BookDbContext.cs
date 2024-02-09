using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Permissions;

namespace PractiseTest1.Entities
{
    public class BookDbContext : DbContext
    {
        private readonly IConfiguration config;

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        public BookDbContext(IConfiguration cfg)
        {
            config = cfg;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(config["ConnString"]);
        }
    }
}
