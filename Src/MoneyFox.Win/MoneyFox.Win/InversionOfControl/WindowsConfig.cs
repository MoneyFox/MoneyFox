namespace MoneyFox.Win.InversionOfControl;

using Core.InversionOfControl;
using Microsoft.Extensions.DependencyInjection;

internal sealed class WindowsConfig
{
    public void Register(IServiceCollection serviceCollection)
    {

        new CoreConfig().Register(serviceCollection);
    }
}
