using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Persistence;

namespace MoneyFox.Application.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    internal static class SqliteEfCoreContextFactory
    {
        public static EfCoreContext Create()
        {
            DbContextOptions<EfCoreContext> options = new DbContextOptionsBuilder<EfCoreContext>()
                                                      .UseSqlite($"Data Source={Guid.NewGuid()}.db")
                                                      .Options;

            var context = new EfCoreContext(options);

            context.Database.EnsureCreated();
            context.SaveChanges();

            return context;
        }

        public static void Destroy(EfCoreContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
