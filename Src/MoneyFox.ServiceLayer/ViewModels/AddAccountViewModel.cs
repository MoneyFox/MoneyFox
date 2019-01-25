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
            base.Prepare();
            SelectedAccount = new AccountViewModel();
        }

        protected override Task SaveAccount()
        {
            throw new System.NotImplementedException();
        }

        protected override Task DeleteAccount()
        {
            throw new System.NotImplementedException();
        }
    }
}
