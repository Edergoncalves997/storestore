using Microsoft.EntityFrameworkCore;
using back_end.Models; // Ajuste conforme o namespace do seu projeto

namespace back_end.Data // Ajuste conforme o namespace do seu projeto
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
