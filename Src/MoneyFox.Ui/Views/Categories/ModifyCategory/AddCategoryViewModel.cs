namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

using Core.ApplicationCore.UseCases.CategoryCreation;
using Core.Common.Interfaces;
using JetBrains.Annotations;
using MediatR;

[UsedImplicitly]
internal sealed class AddCategoryViewModel : ModifyCategoryViewModel
{
    private readonly IMediator mediator;

    public AddCategoryViewModel(IMediator mediator, IDialogService dialogService) : base(mediator: mediator, dialogService: dialogService)
    {
        this.mediator = mediator;

        // TODO: Create a separate create and edit model
        SelectedCategory = new()
        {
            Id = 0,
            Name = string.Empty,
            Note = string.Empty,
            RequireNote = false,
            Created = DateTime.MinValue,
            LastModified = null
        };
    }

    protected override async Task SaveCategoryAsync()
    {
        var command = new CreateCategory.Command(name: SelectedCategory.Name, note: SelectedCategory.Note, requireNote: SelectedCategory.RequireNote);
        await mediator.Send(command);
    }
}
