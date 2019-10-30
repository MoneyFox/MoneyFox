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
            Context = InMemoryEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            InMemoryEfCoreContextFactory.Destroy(Context);
        }
    }
}
