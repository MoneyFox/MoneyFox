namespace MoneyFox.Ui.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using NSubstitute;
using Xunit;

public sealed class ServiceCollectionShould
{
    [Fact]
    public void AllDependenciesPresentAndAccountedFor()
    {
        // Arrange
        ServiceCollection serviceCollection = new();
        _ = serviceCollection.AddSingleton(Substitute.For<IDbPathProvider>());
        _ = serviceCollection.AddSingleton(Substitute.For<IStoreOperations>());
        _ = serviceCollection.AddSingleton(Substitute.For<IAppInformation>());
        _ = serviceCollection.AddSingleton(Substitute.For<IFileStore>());

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

