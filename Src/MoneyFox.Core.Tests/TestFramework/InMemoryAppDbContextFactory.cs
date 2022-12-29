namespace MoneyFox.Core.Tests.TestFramework;

using System.Diagnostics.CodeAnalysis;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
