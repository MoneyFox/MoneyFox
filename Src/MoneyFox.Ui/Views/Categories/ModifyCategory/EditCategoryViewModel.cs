namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Features._Legacy_.Categories.UpdateCategory;
using Core.Features.CategoryDeletion;
using Core.Queries;
using MediatR;
using Resources.Strings;

public class EditCategoryViewModel : ModifyCategoryViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    public EditCategoryViewModel(IMediator mediator, IDialogService dialogService, INavigationService navigationService) : base(
        mediator: mediator,
        dialogService: dialogService)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
    }

    public AsyncRelayCommand DeleteCommand => new(DeleteAsync);

    public async Task InitializeAsync(int categoryId)
    {
        var categoryData = await mediator.Send(new GetCategoryById.Query(categoryId));
        SelectedCategory = new()
        {
            Id = categoryData.Id,
            Name = categoryData.Name,
            Note = categoryData.Note,
            RequireNote = categoryData.NoteRequired,
            Created = categoryData.Created,
            LastModified = categoryData.LastModified
        };
    }

    protected override async Task SaveCategoryAsync()
    {
        // Due to a bug in .net maui, the loading dialog can only be called after any other dialog
        await dialogService.ShowLoadingDialogAsync(Translations.SavingCategoryMessage);
        var command = new UpdateCategory.Command(
            Id: SelectedCategory.Id,
            Name: SelectedCategory.Name,
            Note: SelectedCategory.Note,
            RequireNote: SelectedCategory.RequireNote);

        await mediator.Send(command);
    }

    private async Task DeleteAsync()
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeleteCategoryConfirmationMessage))
        {
            var numberOfAssignedPayments = await mediator.Send(new GetNumberOfPaymentsAssignedToCategory.Query(SelectedCategory.Id));
            if (numberOfAssignedPayments == 0
                || await dialogService.ShowConfirmMessageAsync(
                    title: Translations.RemoveCategoryAssignmentTitle,
                    message: string.Format(format: Translations.RemoveCategoryAssignmentOnPaymentMessage, arg0: numberOfAssignedPayments),
                    positiveButtonText: Translations.RemoveLabel,
                    negativeButtonText: Translations.CancelLabel))
            {
                await mediator.Send(new DeleteCategoryById.Command(SelectedCategory.Id));
                await navigationService.GoBackFromModalAsync();
            }
        }
    }
}
