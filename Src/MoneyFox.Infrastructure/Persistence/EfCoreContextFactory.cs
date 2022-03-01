namespace MoneyFox.Infrastructure.Persistence
{
    using Core._Pending_.Common.Facades;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public static class EfCoreContextFactory
    {
        public static AppDbContext Create(IPublisher publisher, ISettingsFacade settingsFacade, string dbPath)
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            var context = new AppDbContext(options, publisher, settingsFacade);
            context.Database.Migrate();
            return context;
        }
    }
}