namespace MoneyFox.Ui.Tests.ViewModels.SetupAssistant;

using System.Globalization;
using Core.Common.Facades;
using Domain;
using FluentAssertions;
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
        var vm = new SetupCurrencyViewModel(settingsFacade);

        // Assert
        _ = vm.CurrencyViewModels.Should().HaveCount(Currencies.GetAll().Count);
        _ = vm.SelectedCurrency.Should().NotBeNull();
        vm.SelectedCurrency.AlphaIsoCode.Should().Be(RegionInfo.CurrentRegion.ISOCurrencySymbol);
    }
}
