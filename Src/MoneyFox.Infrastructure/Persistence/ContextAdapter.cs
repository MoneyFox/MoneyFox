using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Infrastructure.Persistence
{
    public class ContextAdapter : IContextAdapter
    {
        public IEfCoreContext Context { get; private set; } = EfCoreContextFactory.Create();

        public void RecreateContext()
        {
            Context.Dispose();
            Context = EfCoreContextFactory.Create();
        }
    }
}