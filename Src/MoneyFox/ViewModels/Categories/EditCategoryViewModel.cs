using AutoMapper;
using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Commands.Categories.UpdateCategory;
using MoneyFox.Core.Queries.Categories.GetCategoryById;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Categories
{
    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public EditCategoryViewModel(IMediator mediator,
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