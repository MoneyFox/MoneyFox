namespace MoneyFox.Ui.Tests.Views.SetupAssistant;

using Common.Navigation;
using MediatR;
using MoneyFox.Core.Queries;
using Domain.Aggregates.AccountAggregate;

using Ui.Views.Setup;

public class SetupAccountViewModelTests
{
    [Fact]
    public async void HasAccount()
    {
        // Arrange
        var navigationService = Substitute.For<INavigationService>();
        var mediator = Substitute.For<IMediator>();

        // Act
        var vm = new SetupAccountsViewModel(navigationService, mediator);

        // Assert
        await vm.MadeAccount();
        vm.HasAnyAccount.Should().BeFalse();

        var accounts = new List<Account> { new("TestAccount") };
        mediator.Send(Arg.Any<GetAccountsQuery>()).Returns(accounts);

        await vm.MadeAccount();
        vm.HasAnyAccount.Should().BeTrue();
    }
}
