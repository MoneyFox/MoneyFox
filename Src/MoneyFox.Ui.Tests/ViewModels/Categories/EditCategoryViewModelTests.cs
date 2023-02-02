namespace MoneyFox.Ui.Tests.ViewModels.Categories;

using AutoMapper;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Categories.DeleteCategoryById;
using Core.Interfaces;
using Core.Queries;
using MediatR;
using NSubstitute;
using Views.Categories.ModifyCategory;
using Xunit;

public class EditCategoryViewModelTests
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    private readonly EditCategoryViewModel vm;

    public EditCategoryViewModelTests()
    {
        mediator = Substitute.For<IMediator>();
        var mapper = Substitute.For<IMapper>();
        dialogService = Substitute.For<IDialogService>();
        navigationService = Substitute.For<INavigationService>();
        vm = new(mediator: mediator, mapper: mapper, dialogService: dialogService, navigationService: navigationService);
    }

    [Fact]
    public async Task CallsDelete_WhenConfirmationWasConfirmed()
    {
        // Arrange
        _ = dialogService.ShowConfirmMessageAsync(title: Arg.Any<string>(), message: Arg.Any<string>()).Returns(true);
        _ = mediator.Send(Arg.Any<GetCategoryById.Query>())
        .Returns(
            new CategoryData(
                Id: 4,
                Name: "Beer",
                Note: null,
                NoteRequired: false,
                Created: DateTime.Now,
                LastModified: DateTime.Now));

        // Act
        await vm.InitializeAsync(4);
        await vm.DeleteCommand.ExecuteAsync(null);

        // Assert
        _ = await mediator.Received(1).Send(request: Arg.Any<Command>(), cancellationToken: Arg.Any<CancellationToken>());
        await navigationService.Received(1).GoBackFromModalAsync();
    }

    [Fact]
    public async Task DoesNotDelete_WhenConfirmationWasDeclined()
    {
        // Arrange
        _ = dialogService.ShowConfirmMessageAsync(title: Arg.Any<string>(), message: Arg.Any<string>()).Returns(false);

        // Act
        await vm.DeleteCommand.ExecuteAsync(null);

        // Assert
        _ = await mediator.Received(0).Send(request: Arg.Any<Command>(), cancellationToken: Arg.Any<CancellationToken>());
        await navigationService.Received(0).GoBackFromModalAsync();
    }
}
