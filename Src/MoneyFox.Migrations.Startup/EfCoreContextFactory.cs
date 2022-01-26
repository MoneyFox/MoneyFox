using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MoneyFox.Infrastructure.Persistence;

namespace MoneyFox.Migrations.Startup
{
    public class EfCoreContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite("Data Source=moneyfox.db");

            return new AppDbContext(optionsBuilder.Options, null, null);
        }
    }
}