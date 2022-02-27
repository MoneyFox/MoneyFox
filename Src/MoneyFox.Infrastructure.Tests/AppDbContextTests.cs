namespace MoneyFox.Infrastructure.Tests
{
    using Core._Pending_.Common.Facades;
    using Core.Aggregates;
    using Core.Events;
    using FluentAssertions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using NSubstitute;
    using Persistence;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
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
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options, publisher, settingsFacade);

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
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options, publisher, settingsFacade);
            var account = new Account("Test");

            // Act
            context.Add(account);
            await context.SaveChangesAsync();

            // Assert
            Account loadedAccount = context.Accounts.First();
            loadedAccount.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            account.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
            account.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            await publisher.Received(1).Publish(Arg.Any<DbEntityModifiedEvent>());
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task SetModifiedDateAndSendEventOnSaveChanges()
        {
            // Arrange
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options, publisher, settingsFacade);
            var account = new Account("Test");
            context.Add(account);
            await context.SaveChangesAsync();

            // Act
            account.UpdateAccount("NewTest");
            await context.SaveChangesAsync();

            // Assert
            Account loadedAccount = context.Accounts.First();
            loadedAccount.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            account.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            await publisher.Received().Publish(Arg.Any<DbEntityModifiedEvent>());
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }
    }
}