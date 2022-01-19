using Microsoft.EntityFrameworkCore;
using MoneyFox.Infrastructure.Persistence;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Core.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    internal static class InMemoryEfCoreContextFactory
    {
        public static EfCoreContext Create()
        {
            DbContextOptions<EfCoreContext> options = new DbContextOptionsBuilder<EfCoreContext>()
                                                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                                      .Options;

            var context = new EfCoreContext(options, null, null);

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