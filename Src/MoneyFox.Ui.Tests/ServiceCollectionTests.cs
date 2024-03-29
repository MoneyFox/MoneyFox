namespace MoneyFox.Ui.Tests;

using Aptabase.Maui;
using Core.Common.Settings;
using Core.Features;
using InversionOfControl;
using Microsoft.Identity.Client;

public sealed class ServiceCollectionTests
{
    [Fact]
    public void AllDependenciesPresentAndAccountedFor()
    {
        // Arrange
        var serviceCollection = new ServiceCollection().AddSingleton(Substitute.For<IPublicClientApplication>());

        // Act
        new MoneyFoxConfig().Register(serviceCollection);

        // Add a substitute here, since default is not supported in test
        serviceCollection.AddSingleton(Substitute.For<ISettingsFacade>());
        serviceCollection.AddSingleton(Substitute.For<ISqliteBackupService>());
        serviceCollection.AddSingleton(Substitute.For<IAptabaseClient>());

        // Assert
        List<InvalidOperationException> exceptions = new();
        var provider = serviceCollection.BuildServiceProvider();
        foreach (var serviceDescriptor in serviceCollection)
        {
            var serviceType = serviceDescriptor.ServiceType;
            if (serviceType.Namespace!.StartsWith("MoneyFox") && serviceType.Namespace!.EndsWith("Page"))
            {
                try
                {
                    _ = provider.GetService(serviceType);
                }
                catch (InvalidOperationException e)
                {
                    exceptions.Add(e);
                }
            }
        }

        if (exceptions.Any())
        {
            throw new AggregateException(message: "Some services are missing", innerExceptions: exceptions);
        }
    }
}
