namespace MoneyFox.Ui.Tests.ViewModels;

using AutoMapper;
using Core.Common.Interfaces;
using Core.Queries;
using Domain.Aggregates.CategoryAggregate;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Views.Categories;
using Xunit;

public class CategoryListViewModelTests
{
    private readonly CategoryListViewModel viewModel;
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public CategoryListViewModelTests()
    {
        mediator = Substitute.For<IMediator>();
        mapper = Substitute.For<IMapper>();
        var dialogService = Substitute.For<IDialogService>();
        viewModel = new(mediator: mediator, mapper: mapper, dialogService: dialogService);
    }

    [Fact]
    public void ListNotNullOnCtor()
    {
        // Assert
        viewModel.Categories.Should().NotBeNull();
    }

    [Fact]
    public async Task ItemLoadedInInit()
    {
        // Arrange
        _ = mapper.Map<List<CategoryListItemViewModel>>(Arg.Any<List<Category>>()).Returns(new List<CategoryListItemViewModel> { new() { Name = "asdf" } });

        // Act
        await viewModel.InitializeAsync();

        // Assert
        _ = await mediator.Received(1).Send(Arg.Any<GetCategoryBySearchTermQuery>());
        _ = mapper.Received(1).Map<List<CategoryListItemViewModel>>(Arg.Any<List<Category>>());
    }
}
