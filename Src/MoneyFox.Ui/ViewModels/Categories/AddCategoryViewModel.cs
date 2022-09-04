namespace MoneyFox.Ui.ViewModels.Categories;

using JetBrains.Annotations;
using MediatR;
using MoneyFox.Core.ApplicationCore.UseCases.CategoryCreation;
using MoneyFox.Core.Common.Interfaces;

[UsedImplicitly]
internal sealed class AddCategoryViewModel : ModifyCategoryViewModel
{
    private readonly IMediator mediator;

    public AddCategoryViewModel(IMediator mediator, IDialogService dialogService) : base(mediator: mediator, dialogService: dialogService)
    {
        this.mediator = mediator;
    }

    protected override async Task SaveCategoryAsync()
    {
        var command = new CreateCategory.Command(name: SelectedCategory.Name, note: SelectedCategory.Note, requireNote: SelectedCategory.RequireNote);
        await mediator.Send(command);
    }
}
