using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Helpers;

namespace MoneyFox.Infrastructure.Persistence
{
    public static class EfCoreContextFactory
    {
        public static EfCoreContext Create(IPublisher publisher, ISettingsFacade settingsFacade)
        {
            DbContextOptions<EfCoreContext> options = new DbContextOptionsBuilder<EfCoreContext>()
                                                      .UseSqlite($"Data Source={DatabasePathHelper.DbPath}")
                                                      .Options;

            var context = new EfCoreContext(options, publisher, settingsFacade);
            context.Database.Migrate();
            return context;
        }
    }
}