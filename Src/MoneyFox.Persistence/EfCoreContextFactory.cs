using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common;
using System.IO;
using Windows.Storage;

namespace MoneyFox.Persistence
{
    public static class EfCoreContextFactory
    {
        public static EfCoreContext Create()
        {
            var dbpath = DatabasePathHelper.GetDbPath();

            if (ExecutingPlatform.Current == AppPlatform.UWP)
            {
                dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbpath);
            }

            DbContextOptions<EfCoreContext> options = new DbContextOptionsBuilder<EfCoreContext>()
                                                     .UseSqlite($"Data Source={dbpath}")
                                                     .Options;

            var context = new EfCoreContext(options);
            context.Database.Migrate();
            return context;
        }
    }
}
