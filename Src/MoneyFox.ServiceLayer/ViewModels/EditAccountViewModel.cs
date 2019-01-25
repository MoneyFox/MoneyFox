using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Parameters;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;

        public EditAccountViewModel(ICrudServicesAsync crudServices,
            IDialogService dialogService,
            IMvxLogProvider logProvider, 
            IMvxNavigationService navigationService) : base(crudServices, dialogService, logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;
        }

        public override string Title => string.Format(Strings.EditAccountTitle, SelectedAccount.Name);

        public override async void Prepare(ModifyAccountParameter parameter)
        {
            base.Prepare(parameter);
            SelectedAccount = await crudServices.ReadSingleAsync<AccountViewModel>(AccountId);
        }

        public MvxAsyncCommand DeleteCommand => new MvxAsyncCommand(DeleteAccount);

        protected override async Task SaveAccount()
        {
            await crudServices.UpdateAndSaveAsync(SelectedAccount, "ctor(4)");
            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            await NavigationService.Close(this);
        }

        protected async Task DeleteAccount()
        {
            await crudServices.DeleteAndSaveAsync<AccountViewModel>(SelectedAccount.Id);
        }
    }
}