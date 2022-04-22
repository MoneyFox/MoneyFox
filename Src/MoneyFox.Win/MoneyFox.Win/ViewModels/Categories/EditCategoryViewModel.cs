namespace MoneyFox.Win.ViewModels.Categories;

using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.Aggregates;
using Core.Aggregates.CategoryAggregate;
using Core.Commands.Categories.DeleteCategoryById;
using Core.Commands.Categories.UpdateCategory;
using Core.Common.Interfaces;
using Core.Queries;
using Core.Resources;
using MediatR;
using Services;

public class EditCategoryViewModel : ModifyCategoryViewModel
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public EditCategoryViewModel(IMediator mediator, IDialogService dialogService, INavigationService navigationService, IMapper mapper) : base(
        mediator: mediator,
        navigationService: navigationService,
        mapper: mapper,
        dialogService: dialogService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public AsyncRelayCommand DeleteCommand => new(DeleteCategoryAsync);

    protected override async Task InitializeAsync()
    {
        var category = await mediator.Send(new GetCategoryByIdQuery(CategoryId));
        SelectedCategory = mapper.Map<CategoryViewModel>(category);
        Title = string.Format(provider: CultureInfo.InvariantCulture, format: Strings.EditCategoryTitle, arg0: SelectedCategory.Name);
    }

    protected override async Task SaveCategoryAsync()
    {
        await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
    }

    private async Task DeleteCategoryAsync()
    {
        if (SelectedCategory == null)
        {
            return;
        }

        if (await DialogService.ShowConfirmMessageAsync(title: Strings.DeleteTitle, message: Strings.DeleteCategoryConfirmationMessage))
        {
            await mediator.Send(new DeleteCategoryByIdCommand(SelectedCategory.Id));
            await CancelCommand.ExecuteAsync(null);
        }
    }
}
