using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.Services;
using NLog;
using System.Globalization;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using Windows.UI.WindowManagement;
using MoneyFox.Uwp.Messages;

#nullable enable
namespace MoneyFox.Uwp.ViewModels
{
    public abstract class ModifyAccountViewModel : ViewModelBase
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public int AccountId { get; set; }

        private string title = "";
        private AccountViewModel selectedAccount = new AccountViewModel();

        protected ModifyAccountViewModel(IDialogService dialogService,
                                         INavigationService navigationService)
        {
            DialogService = dialogService;
            NavigationService = navigationService;

            MessengerInstance.Register<AppWindowMessage>(this, (m) => Window = m.AppWindow);
        }

        protected abstract Task SaveAccount();

        protected abstract Task Initialize();

        protected IDialogService DialogService { get; }

        protected INavigationService NavigationService { get; }

        public AppWindow? Window { get; set; }

        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);

        public AsyncCommand SaveCommand => new AsyncCommand(SaveAccountBase);

        public string Title
        {
            get => title;
            set
            {
                if(title == value)
                    return;
                title = value;
                RaisePropertyChanged();
            }
        }

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(AmountString));
            }
        }

        private string amountString = "";

        public string AmountString
        {
            get => amountString;
            set
            {
                if(amountString == value)
                    return;
                amountString = value;
                RaisePropertyChanged();
            }
        }

        private async Task SaveAccountBase()
        {
            if(string.IsNullOrWhiteSpace(SelectedAccount.Name))
            {
                await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if(decimal.TryParse(AmountString, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal convertedValue))
                SelectedAccount.CurrentBalance = convertedValue;
            else
            {
                logManager.Warn($"Amount string {AmountString} could not be parsed to double.");
                await DialogService.ShowMessageAsync(Strings.InvalidNumberTitle, Strings.InvalidNumberCurrentBalanceMessage);
                return;
            }

            await DialogService.ShowLoadingDialogAsync(Strings.SavingAccountMessage);
            await SaveAccount();
            MessengerInstance.Send(new ReloadMessage());
            await DialogService.HideLoadingDialogAsync();
        }
    }
}
