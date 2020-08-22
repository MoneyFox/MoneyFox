using AutoMapper;
using MediatR;
using MoneyFox.Application.Categories.Command.UpdateCategory;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Categories
{
    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public EditCategoryViewModel(IMediator mediator,
                                     IMapper mapper,
                                     IDialogService dialogService)
            : base(mediator, dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task Init(int categoryId)
        {
            SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(categoryId)));
        }

        protected override async Task SaveCategoryAsync()
        {
            await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
        }
    }
}
