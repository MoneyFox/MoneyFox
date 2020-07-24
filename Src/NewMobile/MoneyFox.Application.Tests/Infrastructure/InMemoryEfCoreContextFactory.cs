using Microsoft.EntityFrameworkCore;
using MoneyFox.Persistence;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Application.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    internal static class InMemoryEfCoreContextFactory
    {
        public static EfCoreContext Create()
        {
            DbContextOptions<EfCoreContext> options = new DbContextOptionsBuilder<EfCoreContext>()
                                                     .UseInMemoryDatabase(Guid.NewGuid().ToString())
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
