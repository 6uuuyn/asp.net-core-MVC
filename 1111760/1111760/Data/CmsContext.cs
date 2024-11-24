using _1111760.Data;
using _1111760.Models;
using Microsoft.EntityFrameworkCore;
namespace _1111760.Data
{
    public class CmsContext : DbContext
    {
        

        public CmsContext(DbContextOptions<CmsContext> options) : base(options)
        {
        }

        public DbSet<User> Tablebrands1111760 { get; set; }
        public DbSet<Login> Tableuses1111760 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
