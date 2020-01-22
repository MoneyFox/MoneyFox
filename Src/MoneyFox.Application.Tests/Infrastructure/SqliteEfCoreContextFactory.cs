using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Persistence;

namespace MoneyFox.Application.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    internal static class SQLiteEfCoreContextFactory
    {
        public static EfCoreContext Create()
        {
            DbContextOptions<EfCoreContext> options = new DbContextOptionsBuilder<EfCoreContext>()
                                                     .UseSqlite("Filename=Foo.db")
                                                     .Options;

            var context = new EfCoreContext(options);

            context.Database.Migrate();

            return context;
        }

        public static void Destroy(EfCoreContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
