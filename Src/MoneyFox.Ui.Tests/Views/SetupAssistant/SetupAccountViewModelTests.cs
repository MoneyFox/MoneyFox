namespace MoneyFox.Ui.Tests.Views.SetupAssistant;

using Common.Navigation;
using Core.Queries;
using Domain;
using MediatR;
using Ui.Views.Setup.SetupAccounts;

public class SetupAccountViewModelTests
{
    [Fact]
    public async Task MarkNextStepAsNotAvailableWhenNoAccountWasCreated()
    {
        // Arrange
        var navigationService = Substitute.For<INavigationService>();
        var mediator = Substitute.For<IMediator>();

        // Act
        var vm = new SetupAccountsViewModel(navigationService: navigationService, mediator: mediator);
        await vm.OnNavigatedAsync(null);

        // Assert
        vm.NextStepCommand.CanExecute(null).Should().BeFalse();
    }

    [Fact]
    public async Task MarkNextStepAsAvailableWhenAtLeastOneAccountIsAvailable()
    {
        // Arrange
        var navigationService = Substitute.For<INavigationService>();
        var mediator = Substitute.For<IMediator>();
        var accounts = new List<GetAccountsQuery.AccountData>
        {
            new(Id: 1, Name: "TestAccount", CurrentBalance: Money.Zero(Currencies.USD), IsExcluded: false)
        };

        mediator.Send(Arg.Any<GetAccountsQuery>()).Returns(accounts);

        // Act
        var vm = new SetupAccountsViewModel(navigationService: navigationService, mediator: mediator);
        await vm.OnNavigatedAsync(null);

        // Assert
        vm.NextStepCommand.CanExecute(null).Should().BeTrue();
    }
}
