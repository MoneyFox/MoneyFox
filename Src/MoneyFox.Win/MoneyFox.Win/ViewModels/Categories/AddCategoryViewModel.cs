namespace MoneyFox.Win.ViewModels.Categories;

using AutoMapper;
using Core._Pending_.Common.Interfaces;
using Core.Commands.Categories.CreateCategory;
using Core.Resources;
using MediatR;
using Services;
using System.Threading.Tasks;

public class AddCategoryViewModel : ModifyCategoryViewModel
{
    private readonly IMediator mediator;

    public AddCategoryViewModel(IMediator mediator,
        IDialogService dialogService,
        INavigationService navigationService,
        IMapper mapper) : base(mediator, navigationService, mapper, dialogService)

    {
        this.mediator = mediator;

        Title = Strings.AddCategoryTitle;
    }

    protected override Task InitializeAsync()
    {
        SelectedCategory = new CategoryViewModel();
        return Task.CompletedTask;
    }

    protected override async Task SaveCategoryAsync() =>
        await mediator.Send(
            new CreateCategoryCommand(
                SelectedCategory.Name,
                SelectedCategory.Note,
                SelectedCategory.RequireNote));
}