namespace MoneyFox.Ui.Tests.ViewModels.SetupAssistant;

using System.Globalization;
using Domain;
using FluentAssertions;
using Views.SetupAssistant;
using Xunit;

public class CurrencyIntroductionViewModelTests
{
    [Fact]
    public void CurrenciesAndDefaultItemCorrectlySetOnConstruction()
    {
        // Act
        var vm = new CurrencyIntroductionViewModel();

        // Assert
        vm.CurrencyViewModels.Should().HaveCount(Currencies.GetAll().Count);
        vm.SelectedCurrency.Should().NotBeNull();
        vm.SelectedCurrency.Currency.Should().Be(RegionInfo.CurrentRegion.ISOCurrencySymbol);
    }
}
