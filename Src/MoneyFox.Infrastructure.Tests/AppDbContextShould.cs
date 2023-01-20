namespace MoneyFox.Infrastructure.Tests;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Common.Facades;
using Core.Common.Mediatr;
using Core.Notifications.DatabaseChanged;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Persistence;

public sealed class AppDbContextShould
{
    private readonly ICustomPublisher publisher;
    private readonly ISettingsFacade settingsFacade;

    public AppDbContextShould()
    {
        publisher = Substitute.For<ICustomPublisher>();
        settingsFacade = Substitute.For<ISettingsFacade>();
    }

    [Fact]
    public async Task SetCreatedAndLastModifiedDate_OnEntity_WhenNewEntityIsAdded()
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
        loadedAccount.LastModified.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        account.Created.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        account.LastModified.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        await publisher.Received()
            .Publish(
                notification: Arg.Any<DataBaseChanged.Notification>(),
                strategy: PublishStrategy.ParallelNoWait,
                cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SetModifiedDate_OnEntity_WhenExistingEntityIsUpdated()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new AppDbContext(options: options, publisher: publisher, settingsFacade: settingsFacade);
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
        await publisher.Received()
            .Publish(
                notification: Arg.Any<DataBaseChanged.Notification>(),
                strategy: PublishStrategy.ParallelNoWait,
                cancellationToken: Arg.Any<CancellationToken>());
    }
}
