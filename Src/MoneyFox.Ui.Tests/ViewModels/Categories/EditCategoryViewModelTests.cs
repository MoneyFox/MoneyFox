namespace MoneyFox.Ui.Tests.ViewModels.Categories;

using AutoMapper;
using MediatR;
using MoneyFox.Core.Commands.Categories.DeleteCategoryById;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Interfaces;
using MoneyFox.Ui.ViewModels.Categories;
using NSubstitute;
using Xunit;
using EditCategoryViewModel = Views.Categories.ModifyCategory.EditCategoryViewModel;

public class EditCategoryViewModelTests
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;

    private readonly EditCategoryViewModel vm;

    public EditCategoryViewModelTests()
    {
        mediator = Substitute.For<IMediator>();
        mapper = Substitute.For<IMapper>();
        dialogService = Substitute.For<IDialogService>();
        navigationService = Substitute.For<INavigationService>();

        vm = new(mediator, mapper, dialogService, navigationService);
    }

    [Fact]
    public async Task CallsDelete_WhenConfirmationWasConfirmed()
    {
        // Arrange
        dialogService.ShowConfirmMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        // Act
        await vm.DeleteCommand.ExecuteAsync(null);

        // Assert
        await mediator.Received(1).Send(Arg.Any<DeleteCategoryByIdCommand>(), Arg.Any<CancellationToken>());
        await navigationService.Received(1).GoBackFromModalAsync();
    }

    [Fact]
    public async Task DoesntDelete_WhenConfirmationWasDeclined()
    {
        // Arrange
        dialogService.ShowConfirmMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        // Act
        await vm.DeleteCommand.ExecuteAsync(null);

        // Assert
        await mediator.Received(0).Send(Arg.Any<DeleteCategoryByIdCommand>(), Arg.Any<CancellationToken>());
        await navigationService.Received(0).GoBackFromModalAsync();
    }
}
