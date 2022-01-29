using AutoMapper;
using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Commands.Categories.CreateCategory;
using MoneyFox.Core.Resources;
using MoneyFox.Win.Services;
using System.Threading.Tasks;

namespace MoneyFox.Win.ViewModels.Categories
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;

        public AddCategoryViewModel(IMediator mediator,
            IDialogService dialogService,
            NavigationService navigationService,
            IMapper mapper) : base(mediator, navigationService, mapper, dialogService)

        {
            this.mediator = mediator;

            Title = Strings.AddCategoryTitle;
        }

        protected override Task InitializeAsync()
        {
            SelectedCategory = new CategoryViewModel();
            return Task.CompletedTask;
        }

        protected override async Task SaveCategoryAsync()
        {
            if(string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            await mediator.Send(
                new CreateCategoryCommand(
                    SelectedCategory.Name,
                    SelectedCategory.Note,
                    SelectedCategory.RequireNote));
        }
    }
}