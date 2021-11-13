using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Helpers;

namespace MoneyFox.Infrastructure.Persistence
{
    public static class EfCoreContextFactory
    {
        public static EfCoreContext Create()
        {
            DbContextOptions<EfCoreContext> options = new DbContextOptionsBuilder<EfCoreContext>()
                                                     .UseSqlite($"Data Source={DatabasePathHelper.DbPath}")
                                                     .Options;

            var context = new EfCoreContext(options);
            context.Database.Migrate();
            return context;
        }
    }
}
