using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AddAccountViewModel : ModifyAccountViewModel
    {
        private readonly ICrudServicesAsync crudService;
        private readonly IDialogService dialogService;

        public AddAccountViewModel(ICrudServicesAsync crudService, 
            IDialogService dialogService,
            IMvxLogProvider logProvider, 
            IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.crudService = crudService;
            this.dialogService = dialogService;
        }

        public override void Prepare()
        {
            SelectedAccount = new AccountViewModel();
            base.Prepare();
        }

        protected override async Task SaveAccount()
        {
            await crudService.CreateAndSaveAsync(SelectedAccount, "ctor(4)");
            if (!crudService.IsValid)
            {
                await dialogService.ShowMessage("Errors", crudService.GetAllErrors());
            }

            await NavigationService.Close(this);
        }
    }
}
