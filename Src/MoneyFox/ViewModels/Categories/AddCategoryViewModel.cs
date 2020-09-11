using AutoMapper;
using MediatR;
using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Common.Interfaces;
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
            await mediator.Send(new CreateCategoryCommand(SelectedCategory.Name, SelectedCategory.Note));
        }
    }
}
