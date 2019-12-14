using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Categories.Queries.GetIfCategoryWithNameExists;
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
        private readonly IDialogService dialogService;

        public AddCategoryViewModel(IMediator mediator,
                                    IDialogService dialogService,
                                    ISettingsFacade settingsFacade,
                                    IBackupService backupService,
                                    INavigationService navigationService,
                                    IMapper mapper) : base(mediator, settingsFacade, backupService, navigationService, mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;

            Title = Strings.AddCategoryTitle;
        }

        protected override Task Initialize()
        {
            SelectedCategory = new CategoryViewModel();

            return Task.CompletedTask;
        }

        protected override async Task SaveCategory()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (await mediator.Send(new GetIfCategoryWithNameExistsQuery {CategoryName = SelectedCategory.Name}))
            {
                await dialogService.ShowMessage(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
                return;
            }

            await mediator.Send(new CreateCategoryCommand(mapper.Map<Category>(SelectedCategory)));

            NavigationService.GoBack();
        }
    }
}
