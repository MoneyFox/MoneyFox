namespace MoneyFox.Ui.Tests;

using Core.Common.Settings;
using Core.Interfaces;
using InversionOfControl;
using Microsoft.Identity.Client;
using NSubstitute;
using Xunit;

public sealed class ServiceCollectionTests
{
    [Fact]
    public void AllDependenciesPresentAndAccountedFor()
    {
        // Arrange
        var serviceCollection = new ServiceCollection()
            .AddSingleton(new Lazy<NavigationPage>())
            .AddSingleton(Substitute.For<IDbPathProvider>())
            .AddSingleton(Substitute.For<IFileStore>())
            .AddSingleton(Substitute.For<IPublicClientApplication>());

        // Act
        new MoneyFoxConfig().Register(serviceCollection);

        // Add a substitute here, since default is not supported in test
        serviceCollection.AddSingleton(Substitute.For<ISettingsFacade>());

        // Assert
        List<InvalidOperationException> exceptions = new();
        var provider = serviceCollection.BuildServiceProvider();
        foreach (var serviceDescriptor in serviceCollection)
        {
            var serviceType = serviceDescriptor.ServiceType;
            if (serviceType.Namespace!.StartsWith("MoneyFox"))
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
