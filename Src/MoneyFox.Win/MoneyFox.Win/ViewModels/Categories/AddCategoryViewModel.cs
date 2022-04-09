namespace MoneyFox.Win.ViewModels.Categories;

using System.Threading.Tasks;
using AutoMapper;
using Core.Commands.Categories.CreateCategory;
using Core.Common.Interfaces;
using Core.Resources;
using MediatR;
using Services;

public class AddCategoryViewModel : ModifyCategoryViewModel
{
    private readonly IMediator mediator;

    public AddCategoryViewModel(IMediator mediator, IDialogService dialogService, INavigationService navigationService, IMapper mapper) : base(
        mediator: mediator,
        navigationService: navigationService,
        mapper: mapper,
        dialogService: dialogService)

    {
        this.mediator = mediator;
        Title = Strings.AddCategoryTitle;
    }

    protected override Task InitializeAsync()
    {
        SelectedCategory = new();

        return Task.CompletedTask;
    }

    protected override async Task SaveCategoryAsync()
    {
        await mediator.Send(new CreateCategoryCommand(name: SelectedCategory.Name, note: SelectedCategory.Note, requireNote: SelectedCategory.RequireNote));
    }
}
