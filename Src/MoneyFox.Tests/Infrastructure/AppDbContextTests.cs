namespace MoneyFox.Tests.Infrastructure
{

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.Common.Facades;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using Xunit;

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
        public async Task SetCreatedAndLastModifiedDate_OnSaveChanges()
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
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task SetModifiedDate_OnSaveChanges()
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
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        }
    }

}
