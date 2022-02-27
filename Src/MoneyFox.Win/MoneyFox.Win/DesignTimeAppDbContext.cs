namespace MoneyFox.Win;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MoneyFox.Infrastructure.Persistence;

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