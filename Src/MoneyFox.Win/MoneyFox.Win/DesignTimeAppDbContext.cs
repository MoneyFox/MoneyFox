using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MoneyFox.Core._Pending_.Common.Constants;
using MoneyFox.Infrastructure.Persistence;
using System.IO;
using Windows.Storage;

namespace MoneyFox.Win
{
    public class DesignTimeAppDbContext : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=moneyfox.db}")
                .Options;

            return new AppDbContext(options, null, null);
        }
    }
}