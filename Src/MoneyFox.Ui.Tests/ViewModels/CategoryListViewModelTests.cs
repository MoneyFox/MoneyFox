namespace MoneyFox.Ui.Tests.ViewModels;

using AutoMapper;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries;
using Core.Common.Interfaces;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Views.Categories;
using Xunit;

public class CategoryListViewModelTests
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public CategoryListViewModelTests()
    {
        mediator = Substitute.For<IMediator>();
        mapper = Substitute.For<IMapper>();
        dialogService = Substitute.For<IDialogService>();
    }

    [Fact]
    public void ListNotNullOnCtor()
    {
        // Arrange
        // Act
        var viewModel = new CategoryListViewModel(mediator: mediator, mapper: mapper, dialogService: dialogService);

        // Assert
        viewModel.Categories.Should().NotBeNull();
    }

    [Fact]
    public async Task ItemLoadedInInit()
    {
        // Arrange
        _ = mapper.Map<List<CategoryListItemViewModel>>(Arg.Any<List<Category>>()).Returns(new List<CategoryListItemViewModel> { new() { Name = "asdf" } });
        var viewModel = new CategoryListViewModel(mediator: mediator, mapper: mapper, dialogService: dialogService);

        // Act
        await viewModel.InitializeAsync();

        // Assert
        _ = await mediator.Received(1).Send(Arg.Any<GetCategoryBySearchTermQuery>());
        _ = mapper.Received(1).Map<List<CategoryListItemViewModel>>(Arg.Any<List<Category>>());
    }
}
