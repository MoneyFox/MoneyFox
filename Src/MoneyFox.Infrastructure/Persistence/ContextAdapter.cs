using MediatR;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;

namespace MoneyFox.Infrastructure.Persistence
{
    public class ContextAdapter : IContextAdapter
    {
        private readonly IPublisher publisher;
        private readonly ISettingsFacade settingsFacade;

        public ContextAdapter(IPublisher publisher, ISettingsFacade settingsFacade)
        {
            this.publisher = publisher;
            this.settingsFacade = settingsFacade;

            Context = EfCoreContextFactory.Create(publisher, settingsFacade);
        }

        public IEfCoreContext Context { get; private set; } 

        public void RecreateContext()
        {
            Context.Dispose();
            Context = EfCoreContextFactory.Create(publisher, settingsFacade);
        }
    }
}