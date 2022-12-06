namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries;
using Core.Commands.Categories.DeleteCategoryById;
using Core.Commands.Categories.UpdateCategory;
using Core.Common.Interfaces;
using Core.Interfaces;
using Core.Resources;
using MediatR;

internal sealed partial class EditCategoryViewModel : ModifyCategoryViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    public EditCategoryViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, INavigationService navigationService) : base(
        mediator: mediator,
        dialogService: dialogService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
    }

    public async Task InitializeAsync(int categoryId)
    {
        SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(categoryId)));
    }

    protected override async Task SaveCategoryAsync()
    {
        await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Strings.DeleteTitle, message: Strings.DeleteCategoryConfirmationMessage))
        {
            await mediator.Send(new DeleteCategoryByIdCommand(SelectedCategory.Id));
            await navigationService.GoBackFromModalAsync();
        }
    }
}
