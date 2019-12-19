using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AddCategoryViewModel(IMediator mediator,
                                    IDialogService dialogService,
                                    ISettingsFacade settingsFacade,
                                    IBackupService backupService,
                                    INavigationService navigationService,
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
            NavigationService.GoBack();
        }
    }
}
