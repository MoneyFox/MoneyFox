using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Uwp.Commands;
using MoneyFox.Uwp.Services;
using NLog;
using System.Globalization;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Accounts
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
        }

        protected abstract Task SaveAccountAsync();

        protected abstract Task InitializeAsync();

        protected IDialogService DialogService { get; }

        protected INavigationService NavigationService { get; }

        public AsyncCommand InitializeCommand => new AsyncCommand(InitializeAsync);

        public AsyncCommand SaveCommand => new AsyncCommand(SaveAccountBaseAsync);

        public string Title
        {
            get => title;
            set
            {
                if(title == value)
                {
                    return;
                }

                title = value;
                RaisePropertyChanged();
            }
        }

        public virtual bool IsEdit => false;

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
                {
                    return;
                }

                amountString = value;
                RaisePropertyChanged();
            }
        }

        private async Task SaveAccountBaseAsync()
        {
            if(string.IsNullOrWhiteSpace(SelectedAccount.Name))
            {
                await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if(decimal.TryParse(AmountString, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal convertedValue))
            {
                SelectedAccount.CurrentBalance = convertedValue;
            }
            else
            {
                logManager.Warn($"Amount string {AmountString} could not be parsed to double.");
                await DialogService.ShowMessageAsync(Strings.InvalidNumberTitle,
                    Strings.InvalidNumberCurrentBalanceMessage);
                return;
            }

            await DialogService.ShowLoadingDialogAsync(Strings.SavingAccountMessage);
            await SaveAccountAsync();
            MessengerInstance.Send(new ReloadMessage());
            await DialogService.HideLoadingDialogAsync();
        }
    }
}