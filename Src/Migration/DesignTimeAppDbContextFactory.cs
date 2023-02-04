namespace Migration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MoneyFox.Infrastructure.Persistence;

public class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=moneyfox.db");

        return new(options: optionsBuilder.Options, publisher: null, settingsFacade: null);
    }
}
