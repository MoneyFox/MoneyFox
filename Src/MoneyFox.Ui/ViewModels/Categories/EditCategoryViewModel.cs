namespace MoneyFox.Ui.ViewModels.Categories;

using AutoMapper;
using MediatR;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using MoneyFox.Core.ApplicationCore.Queries;
using MoneyFox.Core.Commands.Categories.UpdateCategory;
using MoneyFox.Core.Common.Interfaces;

internal sealed class EditCategoryViewModel : ModifyCategoryViewModel
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public EditCategoryViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService) : base(mediator: mediator, dialogService: dialogService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public async Task InitializeAsync(int categoryId)
    {
        SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(categoryId)));
    }

    protected override async Task SaveCategoryAsync()
    {
        await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
    }
}
