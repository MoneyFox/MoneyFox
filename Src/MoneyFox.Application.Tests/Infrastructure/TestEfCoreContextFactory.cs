using Microsoft.EntityFrameworkCore;
using MoneyFox.Persistence;
using System;

namespace MoneyFox.Application.Tests.Infrastructure
{
    internal static class TestEfCoreContextFactory
    {
        public static EfCoreContext Create()
        {
            var options = new DbContextOptionsBuilder<EfCoreContext>()
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
