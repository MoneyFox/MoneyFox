namespace MoneyFox.Ui.Tests.Views.SetupAssistant;

using System.Globalization;
using FluentAssertions;
using MoneyFox.Core.Common.Settings;
using MoneyFox.Domain;
using NSubstitute;
using Ui.Views.Setup.SelectCurrency;
using Xunit;

public class CurrencyIntroductionViewModelTests
{
    [Fact]
    public void CurrenciesAndDefaultItemCorrectlySetOnConstruction()
    {
        // Arrange
        var settingsFacade = Substitute.For<ISettingsFacade>();

        // Act
        var vm = new SetupCurrencyViewModel(settingsFacade);

        // Assert
        _ = vm.CurrencyViewModels.Should().HaveCount(Currencies.GetAll().Count);
        _ = vm.SelectedCurrency.Should().NotBeNull();
        vm.SelectedCurrency.AlphaIsoCode.Should().Be(RegionInfo.CurrentRegion.ISOCurrencySymbol);
    }
}
