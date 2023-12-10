namespace MoneyFox.Ui.Tests.Views.SetupAssistant;

using System.Globalization;
using Common.Navigation;
using Core.Common.Settings;
using Domain;
using Ui.Views.Setup.SelectCurrency;

public class CurrencyIntroductionViewModelTests
{
    [Fact]
    public void CurrenciesAndDefaultItemCorrectlySetOnConstruction()
    {
        // Arrange
        var settingsFacade = Substitute.For<ISettingsFacade>();
        var navigationService = Substitute.For<INavigationService>();

        // Act
        var vm = new SetupCurrencyViewModel(settingsFacade, navigationService);

        // Assert
        vm.CurrencyViewModels.Should().HaveCount(Currencies.GetAll().Count);
        vm.SelectedCurrency.Should().NotBeNull();
        vm.SelectedCurrency.AlphaIsoCode.Should().Be(RegionInfo.CurrentRegion.ISOCurrencySymbol);
    }
}
