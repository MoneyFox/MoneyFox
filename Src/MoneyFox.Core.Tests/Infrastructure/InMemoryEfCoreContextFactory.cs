using Microsoft.EntityFrameworkCore;
using MoneyFox.Infrastructure.Persistence;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Core.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    internal static class InMemoryEfCoreContextFactory
    {
        public static AppDbContext Create()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options, null, null);

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
}