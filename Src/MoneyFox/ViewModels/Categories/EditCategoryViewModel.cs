namespace MoneyFox.ViewModels.Categories
{
    using AutoMapper;
    using Core.Aggregates.Payments;
    using Core.Commands.Categories.UpdateCategory;
    using Core.Common.Interfaces;
    using Core.Queries.Categories.GetCategoryById;
    using MediatR;
    using System.Threading.Tasks;

    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public EditCategoryViewModel(
            IMediator mediator,
            IMapper mapper,
            IDialogService dialogService)
            : base(mediator, dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task InitializeAsync(int categoryId) => SelectedCategory =
            mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(categoryId)));

        protected override async Task SaveCategoryAsync() =>
            await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
    }
}