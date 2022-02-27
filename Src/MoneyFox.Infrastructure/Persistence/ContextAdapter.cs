namespace MoneyFox.Infrastructure.Persistence
{
    using Core._Pending_.Common.Facades;
    using Core._Pending_.Common.Interfaces;
    using Core.Interfaces;
    using MediatR;

    public class ContextAdapter : IContextAdapter
    {
        private readonly IPublisher publisher;
        private readonly IDbPathProvider dbPathProvider;
        private readonly ISettingsFacade settingsFacade;

        public ContextAdapter(IPublisher publisher, IDbPathProvider dbPathProvider, ISettingsFacade settingsFacade)
        {
            this.publisher = publisher;
            this.settingsFacade = settingsFacade;

            Context = EfCoreContextFactory.Create(publisher, settingsFacade, dbPathProvider.GetDbPath());
            this.dbPathProvider = dbPathProvider;
        }

        public IAppDbContext Context { get; private set; }

        public void RecreateContext()
        {
            Context.Dispose();
            Context = EfCoreContextFactory.Create(publisher, settingsFacade, dbPathProvider.GetDbPath());
        }
    }
}