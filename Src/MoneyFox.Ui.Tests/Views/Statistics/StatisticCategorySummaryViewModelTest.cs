namespace MoneyFox.Ui.Tests.Views.Statistics;

using System.Reflection;
using FluentAssertions;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Queries.Statistics.GetCategorySummary;
using MoneyFox.Ui.Views.Statistics.CategorySummary;
using NSubstitute;
using Xunit;

public class StatisticCategorySummaryViewModelTest
{
    [Fact]
    public void StatisticCategorySummary_With_Correct_TotalExpense_TotalIncome()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var dialogService = Substitute.For<IDialogService>();
        var vm = new StatisticCategorySummaryViewModel(mediator, dialogService);
        var categorySummaries = new List<CategoryOverviewItem>
        {
            new CategoryOverviewItem { Value = -200 },
            new CategoryOverviewItem { Value = -500 },
            new CategoryOverviewItem { Value = 1000 },
            new CategoryOverviewItem { Value = 1500 }
        };
        var categorySummaryModel = new CategorySummaryModel(default, default, categorySummaries);
        mediator.Send(Arg.Any<GetCategorySummary.Query>(), Arg.Any<CancellationToken>()).Returns(x => Task.FromResult(categorySummaryModel));

        // Act
        vm.LoadedCommand.ExecuteAsync(null);

        // Assert
        vm.TotalExpense.Should().Be(700);
        vm.TotalRevenue.Should().Be(2500);
    }

    [Fact]
    public void StatisticCategorySummary_With_No_Data()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var dialogService = Substitute.For<IDialogService>();
        var vm = new StatisticCategorySummaryViewModel(mediator, dialogService);
        var categorySummaries = new List<CategoryOverviewItem>();
        var categorySummaryModel = new CategorySummaryModel(default, default, categorySummaries);
        mediator.Send(Arg.Any<GetCategorySummary.Query>(), Arg.Any<CancellationToken>()).Returns(x => Task.FromResult(categorySummaryModel));

        // Act
        vm.LoadedCommand.ExecuteAsync(null);

        // Assert
        vm.TotalExpense.Should().Be(0);
        vm.TotalRevenue.Should().Be(0);
    }
}
