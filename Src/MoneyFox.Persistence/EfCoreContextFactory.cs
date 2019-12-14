using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common;

namespace MoneyFox.Persistence
{
    public static class EfCoreContextFactory
    {
        public static EfCoreContext Create()
        {
            DbContextOptions<EfCoreContext> options = new DbContextOptionsBuilder<EfCoreContext>()
                                                      .UseSqlite($"Filename={DatabasePathHelper.GetDbPath()}")
                                                      .Options;

            var context = new EfCoreContext(options);
            context.Database.Migrate();
            return context;
        }
    }
}
