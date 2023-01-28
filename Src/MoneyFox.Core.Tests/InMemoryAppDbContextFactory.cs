namespace MoneyFox.Core.Tests;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
internal static class InMemoryAppDbContextFactory
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new AppDbContext(options: options, publisher: null, settingsFacade: null);
        context.Database.EnsureCreated();
        context.SaveChanges();

        return context;
    }

    public static void Destroy(AppDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
