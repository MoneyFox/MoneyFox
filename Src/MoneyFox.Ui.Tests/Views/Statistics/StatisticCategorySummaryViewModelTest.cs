namespace MoneyFox.Ui.Tests.Views.Statistics;

using Core.Common.Interfaces;
using Core.Queries.Statistics.GetCategorySummary;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Ui.Views.Statistics.CategorySummary;
using Xunit;

public class StatisticCategorySummaryViewModelTest
{
    [Fact]
    public void StatisticCategorySummary_With_Correct_TotalExpense_TotalIncome()
    {
        // Arrange
        var mediator = Substitute.For<IMediator>();
        var dialogService = Substitute.For<IDialogService>();
        var vm = new StatisticCategorySummaryViewModel(mediator: mediator, dialogService: dialogService);
        var categorySummaries = new List<CategoryOverviewItem>
        {
            new() { Value = -200 },
            new() { Value = -500 },
            new() { Value = 1000 },
            new() { Value = 1500 }
        };

        var categorySummaryModel = new CategorySummaryModel(totalEarned: default, totalSpent: default, categoryOverviewItems: categorySummaries);
        mediator.Send(request: Arg.Any<GetCategorySummary.Query>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(categorySummaryModel));

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
        var vm = new StatisticCategorySummaryViewModel(mediator: mediator, dialogService: dialogService);
        var categorySummaries = new List<CategoryOverviewItem>();
        var categorySummaryModel = new CategorySummaryModel(totalEarned: default, totalSpent: default, categoryOverviewItems: categorySummaries);
        mediator.Send(request: Arg.Any<GetCategorySummary.Query>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(categorySummaryModel));

        // Act
        vm.LoadedCommand.ExecuteAsync(null);

        // Assert
        vm.TotalExpense.Should().Be(0);
        vm.TotalRevenue.Should().Be(0);
    }
}
