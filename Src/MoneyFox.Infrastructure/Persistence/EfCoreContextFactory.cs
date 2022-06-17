namespace MoneyFox.Infrastructure.Persistence
{

    using Core._Pending_.Common.Facades;
    using Core.Common.Mediatr;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public static class EfCoreContextFactory
    {
        public static AppDbContext Create(ICustomPublisher publisher, ISettingsFacade settingsFacade, string dbPath)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={dbPath}").Options;
            var context = new AppDbContext(options: options, publisher: publisher, settingsFacade: settingsFacade);
            context.Database.Migrate();

            return context;
        }
    }

}
