namespace MoneyFox.Infrastructure.Tests;

using Core.Common.Settings;
using Core.Notifications.DatabaseChanged;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Persistence;

public sealed class AppDbContextTests
{
    private readonly IPublisher publisher = Substitute.For<IPublisher>();
    private readonly ISettingsFacade settingsFacade = Substitute.For<ISettingsFacade>();

    [Fact]
    public async Task SetCreatedAndLastModifiedDate_OnEntity_WhenNewEntityIsAdded()
    {
        // Arrange
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        var context = new AppDbContext(options: options, publisher: publisher, settingsFacade: settingsFacade);
        await context.Database.EnsureCreatedAsync();
        var account = new Account("Test");

        // Act
        context.Add(account);
        await context.SaveChangesAsync();

        // Assert
        var loadedAccount = context.Accounts.First();
        loadedAccount.Created.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        loadedAccount.LastModified.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        account.Created.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        account.LastModified.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        await publisher.Received().Publish(notification: Arg.Any<DataBaseChanged.Notification>(), cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SetModifiedDate_OnEntity_WhenExistingEntityIsUpdated()
    {
        // Arrange
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        var context = new AppDbContext(options: options, publisher: publisher, settingsFacade: settingsFacade);
        await context.Database.EnsureCreatedAsync();
        var account = new Account("Test");
        context.Add(account);
        await context.SaveChangesAsync();

        // Act
        account.Change("NewTest");
        await context.SaveChangesAsync();

        // Assert
        var loadedAccount = context.Accounts.First();
        loadedAccount.LastModified.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        account.LastModified.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        await publisher.Received().Publish(notification: Arg.Any<DataBaseChanged.Notification>(), cancellationToken: Arg.Any<CancellationToken>());
    }
}
