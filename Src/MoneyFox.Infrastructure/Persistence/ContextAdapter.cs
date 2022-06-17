namespace MoneyFox.Infrastructure.Persistence
{

    using Core._Pending_.Common.Facades;
    using Core.Common.Interfaces;
    using Core.Common.Mediatr;
    using Core.Interfaces;
    using MediatR;

    public class ContextAdapter : IContextAdapter
    {
        private readonly ICustomPublisher publisher;
        private readonly IDbPathProvider dbPathProvider;
        private readonly ISettingsFacade settingsFacade;

        public ContextAdapter(ICustomPublisher publisher, IDbPathProvider dbPathProvider, ISettingsFacade settingsFacade)
        {
            this.publisher = publisher;
            this.settingsFacade = settingsFacade;
            Context = EfCoreContextFactory.Create(publisher: publisher, settingsFacade: settingsFacade, dbPath: dbPathProvider.GetDbPath());
            this.dbPathProvider = dbPathProvider;
        }

        public IAppDbContext Context { get; private set; }

        public void RecreateContext()
        {
            Context.Dispose();
            Context = EfCoreContextFactory.Create(publisher: publisher, settingsFacade: settingsFacade, dbPath: dbPathProvider.GetDbPath());
        }
    }

}
