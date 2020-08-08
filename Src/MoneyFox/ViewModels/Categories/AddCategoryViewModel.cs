using AutoMapper;
using MediatR;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.ViewModels.Categories
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;

        public AddCategoryViewModel(IMediator mediator,
                                    IMapper mapper,
                                    IDialogService dialogService)

        {
            this.mediator = mediator;
            this.mapper = mapper;
        }
    }
}
