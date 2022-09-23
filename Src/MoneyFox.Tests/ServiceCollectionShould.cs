namespace MoneyFox.Tests
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using InversionOfControl;
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
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(Substitute.For<IDbPathProvider>());
            serviceCollection.AddSingleton(Substitute.For<IStoreOperations>());
            serviceCollection.AddSingleton(Substitute.For<IAppInformation>());
            serviceCollection.AddSingleton(Substitute.For<IFileStore>());

            // Act
            new MoneyFoxConfig().Register(serviceCollection);

            // Assert
            var exceptions = new List<InvalidOperationException>();
            var provider = serviceCollection.BuildServiceProvider();
            foreach (var serviceDescriptor in serviceCollection)
            {
                var serviceType = serviceDescriptor.ServiceType;
                if (serviceType.Namespace!.StartsWith("MoneyFox"))
                {
                    try
                    {
                        provider.GetService(serviceType);
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

}
