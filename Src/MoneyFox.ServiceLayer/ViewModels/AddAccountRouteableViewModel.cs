using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.QueryObject;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels {
    public class AddAccountRouteableViewModel : ModifyAccountRouteableViewModel {
        private readonly ICrudServicesAsync crudService;
        private readonly IDialogService dialogService;

        public AddAccountRouteableViewModel(IScreen hostScreen,
                                   ICrudServicesAsync crudService = null,
                                   ISettingsFacade settingsFacade = null,
                                   IBackupService backupService = null,
                                   IDialogService dialogService = null) 
            : base(settingsFacade, backupService)
        {
            HostScreen = hostScreen;

            this.crudService = crudService;
            this.dialogService = dialogService;
        }

        public override string UrlPathSegment => "AddAccount";
        public override IScreen HostScreen { get; }

        //public override void Prepare(ModifyAccountParameter parameter)
        //{
        //    SelectedAccount = new AccountViewModel();
        //    base.Prepare(parameter);
        //}

        protected override async Task SaveAccount()
        {
            if (await crudService.ReadManyNoTracked<AccountViewModel>()
                                 .AnyWithNameAsync(SelectedAccount.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            await crudService.CreateAndSaveAsync(SelectedAccount, "ctor(4)");
            if (!crudService.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudService.GetAllErrors());
            }

            HostScreen.Router.NavigateBack.Execute();
        }
    }
}