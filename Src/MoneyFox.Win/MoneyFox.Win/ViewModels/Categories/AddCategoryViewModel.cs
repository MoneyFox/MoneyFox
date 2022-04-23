namespace MoneyFox.Win.ViewModels.Categories;

using System.Threading.Tasks;
using AutoMapper;
using Core.ApplicationCore.UseCases.CategoryCreation;
using Core.Common.Interfaces;
using Core.Resources;
using JetBrains.Annotations;
using MediatR;
using Services;

[UsedImplicitly]
internal sealed class AddCategoryViewModel : ModifyCategoryViewModel
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
        var command = new CreateCategory.Command(name: SelectedCategory.Name, note: SelectedCategory.Note, requireNote: SelectedCategory.RequireNote);
        await mediator.Send(command);
    }
}
