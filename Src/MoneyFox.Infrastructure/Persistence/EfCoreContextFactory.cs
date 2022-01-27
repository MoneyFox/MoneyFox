using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Helpers;

namespace MoneyFox.Infrastructure.Persistence
{
    public static class EfCoreContextFactory
    {
        public static AppDbContext Create(IPublisher publisher, ISettingsFacade settingsFacade)
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={DatabasePathHelper.DbPath}")
                .Options;

            var context = new AppDbContext(options, publisher, settingsFacade);
            context.Database.Migrate();
            return context;
        }
    }
}