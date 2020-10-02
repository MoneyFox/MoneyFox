using AutoMapper;
using MediatR;
using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Ui.Shared.ViewModels.Categories;
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
            if(string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if(string.IsNullOrEmpty(SelectedCategory.Note))
            {
                await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NoteRequiredMessage);
                return;
            }

            await mediator.Send(new CreateCategoryCommand(SelectedCategory.Name, SelectedCategory.Note));
        }
    }
}
