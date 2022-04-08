namespace MoneyFox.Win.ViewModels.Categories;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.Aggregates.Payments;
using Core.Commands.Categories.DeleteCategoryById;
using Core.Commands.Categories.UpdateCategory;
using Core.Common.Interfaces;
using Core.Resources;
using MediatR;
using NLog;
using Services;
using System.Globalization;
using System.Threading.Tasks;
using Core.Queries;

public class EditCategoryViewModel : ModifyCategoryViewModel
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public EditCategoryViewModel(IMediator mediator,
        IDialogService dialogService,
        INavigationService navigationService,
        IMapper mapper) : base(mediator, navigationService, mapper, dialogService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public AsyncRelayCommand DeleteCommand => new(DeleteCategoryAsync);

    protected override async Task InitializeAsync()
    {
        Category category = await mediator.Send(new GetCategoryByIdQuery(CategoryId));
        SelectedCategory = mapper.Map<CategoryViewModel>(category);
        Title = string.Format(CultureInfo.InvariantCulture, Strings.EditCategoryTitle, SelectedCategory.Name);
    }

    protected override async Task SaveCategoryAsync() =>
        await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));

    private async Task DeleteCategoryAsync()
    {
        if(SelectedCategory == null)
        {
            return;
        }

        if(await DialogService.ShowConfirmMessageAsync(
               Strings.DeleteTitle,
               Strings.DeleteCategoryConfirmationMessage))
        {
            await mediator.Send(new DeleteCategoryByIdCommand(SelectedCategory.Id));
            await CancelCommand.ExecuteAsync(null);
        }
    }
}
