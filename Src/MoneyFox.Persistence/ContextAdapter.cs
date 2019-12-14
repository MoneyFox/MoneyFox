using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Persistence
{
    public class ContextAdapter : IContextAdapter
    {
        private IEfCoreContext context;

        public IEfCoreContext Context
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

        public void RecreateContext()
        {
            context.
        }
    }
}
