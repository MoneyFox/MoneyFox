namespace MoneyFox.Ui.Tests.ViewModels.Categories;

using AutoMapper;
using Core.ApplicationCore.Queries;
using Core.Interfaces;
using MediatR;
using MoneyFox.Core.Commands.Categories.DeleteCategoryById;
using MoneyFox.Core.Common.Interfaces;
using NSubstitute;
using Views.Categories.ModifyCategory;
using Xunit;

public class EditCategoryViewModelTests
{
    private readonly IMediator mediator;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;

    private readonly EditCategoryViewModel vm;

    public EditCategoryViewModelTests()
    {
        mediator = Substitute.For<IMediator>();
        IMapper mapper = Substitute.For<IMapper>();
        dialogService = Substitute.For<IDialogService>();
        navigationService = Substitute.For<INavigationService>();

        vm = new(mediator, mapper, dialogService, navigationService);
    }

    [Fact]
    public async Task CallsDelete_WhenConfirmationWasConfirmed()
    {
        // Arrange
        _ = dialogService.ShowConfirmMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        _ = mediator.Send(Arg.Any<GetCategoryById.Query>())
        .Returns(
            new CategoryData(
                4,
                "Beer",
                null,
                false,
                DateTime.Now,
                DateTime.Now));

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
