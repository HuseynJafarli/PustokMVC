using Microsoft.EntityFrameworkCore;
using Pustok.Core.Models;

namespace Pustok.Data.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Slide> Slides { get; set; }
    }
}
