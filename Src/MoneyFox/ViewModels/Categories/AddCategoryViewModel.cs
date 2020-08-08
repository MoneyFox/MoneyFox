using AutoMapper;
using MediatR;
using MoneyFox.Application.Categories.Command.UpdateCategory;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Categories
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AddCategoryViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IDialogService dialogService) : base(mediator, dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        protected override async Task SaveCategoryAsync()
        {
            await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
        }
    }
}
