using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Persistence
{
    public class ContextAdapter : IContextAdapter
    {
        private IEfCoreContextAdapter context;

        public IEfCoreContextAdapter Context
        {
            get
            {
                if(context == null)
                {
                    context = EfCoreContextFactory.Create();
                }
                return context;
            }
        }

        public void RecreateContext() => context = EfCoreContextFactory.Create();
    }
}
