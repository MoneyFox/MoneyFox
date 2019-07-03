using MoneyFox.Persistence;

namespace MoneyFox.Application.Tests.Infrastructure
{
    internal class CommandTestBase
    {
        protected readonly EfCoreContext Context;

        public CommandTestBase()
        {
            Context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(Context);
        }
    }
}
