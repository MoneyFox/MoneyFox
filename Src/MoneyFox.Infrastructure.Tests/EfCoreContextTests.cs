using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Events;
using MoneyFox.Infrastructure.Persistence;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Infrastructure.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class EfCoreContextTests
    {
        private readonly IPublisher publisher;
        private readonly  ISettingsFacade settingsFacade;

        public EfCoreContextTests()
        {
            publisher = Substitute.For<IPublisher>();
            settingsFacade = Substitute.For<ISettingsFacade>();
        }
        
        [Fact]
        public async Task DoesNotSendEvent_WhenNothingSaved()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EfCoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new EfCoreContext(options, publisher, settingsFacade);

            // Act
            await context.SaveChangesAsync();
            
            // Assert
            await publisher.DidNotReceive().Publish(Arg.Any<DbEntityModifiedEvent>());
            _ = settingsFacade.DidNotReceive().LastDatabaseUpdate;
        }
        
        [Fact]
        public async Task SetCreatedDateAndSendEventOnSaveChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EfCoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new EfCoreContext(options, publisher, settingsFacade);
            var account = new Account("Test");
            
            // Act
            context.Add(account);
            await context.SaveChangesAsync();
            
            // Assert
            var loadedAccount = context.Accounts.First();
            loadedAccount.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
            
            account.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
            
            await publisher.Received(1).Publish(Arg.Any<DbEntityModifiedEvent>());
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }
        
        [Fact]
        public async Task SetModifiedDateAndSendEventOnSaveChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EfCoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new EfCoreContext(options, publisher, settingsFacade);
            var account = new Account("Test");
            context.Add(account);
            await context.SaveChangesAsync();
            
            // Act
            account.UpdateAccount("NewTest");
            await context.SaveChangesAsync();
            
            // Assert
            var loadedAccount = context.Accounts.First();
            loadedAccount.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
            
            account.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
            
            await publisher.Received().Publish(Arg.Any<DbEntityModifiedEvent>());
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }
    }
}