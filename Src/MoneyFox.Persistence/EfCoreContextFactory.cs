using System;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataLayer;

namespace MoneyFox.Persistence
{
    public static class EfCoreContextFactory
    {
        public static EfCoreContext Create()
        {
            var options = new DbContextOptionsBuilder<EfCoreContext>()
                          .UseSqlite($"Filename={DatabasePathHelper.GetDbPath()}")
                          .Options;

            var context = new EfCoreContext(options);

            context.Database.Migrate();

            return context;
        }
    }
}
