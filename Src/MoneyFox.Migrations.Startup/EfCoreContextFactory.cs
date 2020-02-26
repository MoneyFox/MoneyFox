using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MoneyFox.Persistence;

namespace MoneyFox.Migrations.Startup
{
    public class EfCoreContextFactory : IDesignTimeDbContextFactory<EfCoreContext>
    {
        public EfCoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfCoreContext>();
            optionsBuilder.UseSqlite("Data Source=moneyfox.db");

            return new EfCoreContext(optionsBuilder.Options);
        }
    }
}
