using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.Messages;
using MoneyFox.Core.Resources;
using MoneyFox.Win.Services;
using NLog;
using System.Globalization;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Win.ViewModels.Accounts
{
    public abstract class ModifyAccountViewModel : ObservableRecipient
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public int AccountId { get; set; }

        private string title = "";
        private AccountViewModel selectedAccount = new AccountViewModel();

        protected ModifyAccountViewModel(
            IDialogService dialogService,
            INavigationService navigationService)
        {
            DialogService = dialogService;
            NavigationService = navigationService;
        }

        protected abstract Task SaveAccountAsync();

        protected abstract Task InitializeAsync();

        protected IDialogService DialogService { get; }

        protected INavigationService NavigationService { get; }

        public AsyncRelayCommand InitializeCommand => new AsyncRelayCommand(InitializeAsync);

        public AsyncRelayCommand SaveCommand => new AsyncRelayCommand(SaveAccountBaseAsync);

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
                OnPropertyChanged();
            }
        }

        public virtual bool IsEdit => false;

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AmountString));
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
                OnPropertyChanged();
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
                await DialogService.ShowMessageAsync(
                    Strings.InvalidNumberTitle,
                    Strings.InvalidNumberCurrentBalanceMessage);
                return;
            }

            await DialogService.ShowLoadingDialogAsync(Strings.SavingAccountMessage);
            await SaveAccountAsync();
            Messenger.Send(new ReloadMessage());
            await DialogService.HideLoadingDialogAsync();
        }
    }
}