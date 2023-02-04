namespace MoneyFox.Ui.Tests.ViewModels.SetupAssistant;

using System.Globalization;
using Domain;
using FluentAssertions;
using MoneyFox.Core.Common.Facades;
using MoneyFox.Ui.Views.SetupAssistant.CurrencyIntroduction;
using NSubstitute;
using Xunit;

public class CurrencyIntroductionViewModelTests
{
    [Fact]
    public void CurrenciesAndDefaultItemCorrectlySetOnConstruction()
    {
        // Arrange
        var settingsFacade = Substitute.For<ISettingsFacade>();

        // Act
        var vm = new CurrencyIntroductionViewModel(settingsFacade);

        // Assert
        _ = vm.CurrencyViewModels.Should().HaveCount(Currencies.GetAll().Count);
        _ = vm.SelectedCurrency.Should().NotBeNull();
        vm.SelectedCurrency.AlphaIsoCode.Should().Be(RegionInfo.CurrentRegion.ISOCurrencySymbol);
    }
}
