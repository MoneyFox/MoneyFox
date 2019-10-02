using System.Diagnostics.CodeAnalysis;
using MoneyFox.Persistence;

namespace MoneyFox.Application.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
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
