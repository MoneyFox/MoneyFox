namespace MoneyFox.Ui.Tests.ViewModels.Categories;

using AutoMapper;
using MediatR;
using MoneyFox.Core.Commands.Categories.DeleteCategoryById;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using NSubstitute;
using Xunit;
using EditCategoryViewModel = Views.Categories.ModifyCategory.EditCategoryViewModel;

public class EditCategoryViewModelTests
{
    private readonly IMediator mediator;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;

    private readonly EditCategoryViewModel vm;

    public EditCategoryViewModelTests()
    {
        mediator = Substitute.For<IMediator>();
        var mapper = Substitute.For<IMapper>();
        dialogService = Substitute.For<IDialogService>();
        navigationService = Substitute.For<INavigationService>();

        vm = new(mediator, mapper, dialogService, navigationService);
    }

    [Fact]
    public async Task CallsDelete_WhenConfirmationWasConfirmed()
    {
        // Arrange
        _ = dialogService.ShowConfirmMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        // Act
        await vm.DeleteCommand.ExecuteAsync(null);

        // Assert
        _ = await mediator.Received(1).Send(Arg.Any<DeleteCategoryByIdCommand>(), Arg.Any<CancellationToken>());
        await navigationService.Received(1).GoBackFromModalAsync();
    }

    [Fact]
    public async Task DoesNotDelete_WhenConfirmationWasDeclined()
    {
        // Arrange
        _ = dialogService.ShowConfirmMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        // Act
        await vm.DeleteCommand.ExecuteAsync(null);

        // Assert
        _ = await mediator.Received(0).Send(Arg.Any<DeleteCategoryByIdCommand>(), Arg.Any<CancellationToken>());
        await navigationService.Received(0).GoBackFromModalAsync();
    }
}
