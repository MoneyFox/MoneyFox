using AutoMapper;
using MediatR;
using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Uwp.Services;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AddCategoryViewModel(IMediator mediator,
                                    IDialogService dialogService,
                                    NavigationService navigationService,
                                    IMapper mapper) : base(mediator, navigationService, mapper, dialogService)

        {
            this.mediator = mediator;
            this.mapper = mapper;

            Title = Strings.AddCategoryTitle;
        }

        protected override Task InitializeAsync()
        {
            SelectedCategory = new CategoryViewModel();
            return Task.CompletedTask;
        }

        protected override async Task SaveCategoryAsync()
        {
            await mediator.Send(new CreateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
        }
    }
}
