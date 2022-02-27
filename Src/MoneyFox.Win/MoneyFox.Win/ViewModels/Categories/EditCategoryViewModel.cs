namespace MoneyFox.Win.ViewModels.Categories;

using CommunityToolkit.Mvvm.Input;
using Core._Pending_.Common.Interfaces;
using Core.Aggregates.Payments;
using Core.Commands.Categories.DeleteCategoryById;
using Core.Commands.Categories.UpdateCategory;
using Core.Queries.Categories.GetCategoryById;
using Core.Resources;
using global::AutoMapper;
using MediatR;
using NLog;
using Services;
using System.Globalization;
using System.Threading.Tasks;

public class EditCategoryViewModel : ModifyCategoryViewModel
{
    private readonly Logger logManager = LogManager.GetCurrentClassLogger();

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

    /// <summary>
    ///     Delete the selected CategoryViewModel from the database
    /// </summary>
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
            logManager.Info("Category with Id {id} deleted.", SelectedCategory.Id);
            await CancelCommand.ExecuteAsync(null);
        }
    }
}