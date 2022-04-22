namespace MoneyFox.Infrastructure.Tests
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Core._Pending_.Common.Facades;
    using Core.Aggregates;
    using Core.Aggregates.AccountAggregate;
    using Core.Events;
    using FluentAssertions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using NSubstitute;
    using Persistence;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public sealed class AppDbContextTests
    {
        private readonly IPublisher publisher;
        private readonly ISettingsFacade settingsFacade;

        public AppDbContextTests()
        {
            publisher = Substitute.For<IPublisher>();
            settingsFacade = Substitute.For<ISettingsFacade>();
        }

        [Fact]
        public async Task DoesNotSendEvent_WhenNothingSaved()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new AppDbContext(options: options, publisher: publisher, settingsFacade: settingsFacade);

            // Act
            await context.SaveChangesAsync();

            // Assert
            await publisher.DidNotReceive().Publish(Arg.Any<DbEntityModifiedEvent>());
            _ = settingsFacade.DidNotReceive().LastDatabaseUpdate;
        }

        [Fact]
        public async Task SetCreatedAndLastModifiedDateAndSendEventOnSaveChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new AppDbContext(options: options, publisher: publisher, settingsFacade: settingsFacade);
            var account = new Account("Test");

            // Act
            context.Add(account);
            await context.SaveChangesAsync();

            // Assert
            var loadedAccount = context.Accounts.First();
            loadedAccount.Created.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
            account.Created.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
            account.LastModified.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
            await publisher.Received(1).Publish(Arg.Any<DbEntityModifiedEvent>());
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task SetModifiedDateAndSendEventOnSaveChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new AppDbContext(options: options, publisher: publisher, settingsFacade: settingsFacade);
            var account = new Account("Test");
            context.Add(account);
            await context.SaveChangesAsync();

            // Act
            account.UpdateAccount("NewTest");
            await context.SaveChangesAsync();

            // Assert
            var loadedAccount = context.Accounts.First();
            loadedAccount.LastModified.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
            account.LastModified.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
            await publisher.Received().Publish(Arg.Any<DbEntityModifiedEvent>());
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        }
    }

}
