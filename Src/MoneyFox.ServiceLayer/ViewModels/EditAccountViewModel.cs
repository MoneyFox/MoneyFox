using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Parameters;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly ICrudServices crudService;
        private IDialogService dialogService;

        public EditAccountViewModel(ICrudServices crudService,
            IDialogService dialogService,
            IMvxLogProvider logProvider, 
            IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.crudService = crudService;
            this.dialogService = dialogService;
        }

        public override string Title => string.Format(Strings.EditAccountTitle, SelectedAccount.Name);

        public override void Prepare(ModifyAccountParameter parameter)
        {
            base.Prepare(parameter);
            SelectedAccount = crudService.ReadSingle<AccountViewModel>(AccountId);
            Amount = SelectedAccount.CurrentBalance;
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