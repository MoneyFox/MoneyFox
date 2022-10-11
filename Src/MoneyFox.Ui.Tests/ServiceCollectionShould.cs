namespace MoneyFox.Ui.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using MoneyFox.Ui.InversionOfControl;
using NSubstitute;
using Xunit;

public sealed class ServiceCollectionShould
{
    [Fact]
    public void AllDependenciesPresentAndAccountedFor()
    {
        // Arrange
        var serviceCollection = new ServiceCollection()
            .AddSingleton(Substitute.For<IDbPathProvider>())
            .AddSingleton(Substitute.For<IStoreOperations>())
            .AddSingleton(Substitute.For<IAppInformation>())
            .AddSingleton(Substitute.For<IFileStore>())
            .AddSingleton(Substitute.For<IPublicClientApplication>());

        // Act
        new MoneyFoxConfig().Register(serviceCollection);

        // Assert
        List<InvalidOperationException> exceptions = new();
        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        foreach (ServiceDescriptor serviceDescriptor in serviceCollection)
        {
            Type serviceType = serviceDescriptor.ServiceType;
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

