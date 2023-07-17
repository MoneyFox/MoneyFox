namespace MoneyFox.Ui.Tests.Views.Statistics;

using System.Collections.ObjectModel;
using FluentAssertions;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Ui.Views.Statistics;
using MoneyFox.Ui.Views.Statistics.CategorySummary;
using NSubstitute;
using Xunit;

public class StatisticCategorySummaryViewModelTest
{
    [Fact]
    public void StatisticCategorySummary_With_Correct_TotalExpense_TotalIncome()
    {
        // Arrange
        var dialogService = Substitute.For<IDialogService>();
        var mediator = Substitute.For<IMediator>();
        var vm = new StatisticCategorySummaryViewModel(mediator, dialogService);

        // Act
        vm.CategorySummary = new ObservableCollection<CategoryOverviewViewModel>
        {
            new CategoryOverviewViewModel
            {
                CategoryId = 1,
                Value = -200
            },
            new CategoryOverviewViewModel
            {
                CategoryId = 2,
                Value = -500
            },
            new CategoryOverviewViewModel
            {
                CategoryId = 3,
                Value = 1000
            },
            new CategoryOverviewViewModel
            {
                CategoryId = 4,
                Value = 1500
            }
        };

        // Assert
        vm.TotalExpense.Should().Be(700);
        vm.TotalIncome.Should().Be(2500);
    }
}
