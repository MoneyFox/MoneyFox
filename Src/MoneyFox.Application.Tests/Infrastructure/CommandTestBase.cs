using MoneyFox.Persistence;

namespace MoneyFox.Application.Tests.Infrastructure
{
    public class CommandTestBase
    {
        protected readonly EfCoreContext context;

        public CommandTestBase()
        {
            context = EfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            EfCoreContextFactory.Destroy(context);
        }
    }
}
