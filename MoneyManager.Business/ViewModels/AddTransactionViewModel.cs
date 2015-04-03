using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyManager.Business.Logic;
using MoneyManager.Business.Manager;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class AddTransactionViewModel {
        private readonly IAccountRepository _accountRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly CurrencyManager _currencyManager;
        private readonly SettingDataAccess _settings;
        private readonly ITransactionRepository _transactionRepository;

        public AddTransactionViewModel(ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            IRepository<Category> categoryRepository,
            CurrencyManager currencyManager,
            SettingDataAccess settings) {
            _transactionRepository = transactionRepository;
            _currencyManager = currencyManager;
            _settings = settings;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            IsNavigationBlocked = true;
        }

        public bool IsNavigationBlocked { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEndless { get; set; }
        public bool IsEdit { get; set; }
        public int Recurrence { get; set; }
        public bool IsTransfer { get; set; }
        public bool RefreshRealtedList { get; set; }

        public FinancialTransaction SelectedTransaction {
            get { return _transactionRepository.Selected; }
            set { _transactionRepository.Selected = value; }
        }

        public ObservableCollection<Account> AllAccounts {
            get { return _accountRepository.Data; }
        }

        public ObservableCollection<Category> AllCategories {
            get { return _categoryRepository.Data; }
        }

        public string Title {
            get {
                var text = IsEdit
                    ? Translation.GetTranslation("EditTitle")
                    : Translation.GetTranslation("AddTitle");

                var type = TransactionTypeLogic.GetViewTitleForType(_transactionRepository.Selected.Type);

                return String.Format(text, type);
            }
        }

        public string AmountString {
            get { return AmountWithoutExchange.ToString(); }
            set {
                double amount;
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentUICulture, out amount)) {
                    AmountWithoutExchange = amount;
                }
            }
        }

        public double AmountWithoutExchange {
            get { return _transactionRepository.Selected.AmountWithoutExchange; }
            set {
                _transactionRepository.Selected.AmountWithoutExchange = value;
                CalculateNewAmount(value);
            }
        }

        private void CalculateNewAmount(double value) {
            if (Math.Abs(_transactionRepository.Selected.ExchangeRatio) < 0.5) {
                _transactionRepository.Selected.ExchangeRatio = 1;
            }

            _transactionRepository.Selected.Amount = _transactionRepository.Selected.ExchangeRatio*value;
        }

        public async void SetCurrency(string currency) {
            _transactionRepository.Selected.Currency = currency;
            await LoadCurrencyRatio();
            _transactionRepository.Selected.IsExchangeModeActive = true;
            CalculateNewAmount(AmountWithoutExchange);
        }

        public async Task LoadCurrencyRatio() {
            _transactionRepository.Selected.ExchangeRatio =
                await
                    _currencyManager.GetCurrencyRatio(_settings.DefaultCurrency,
                        _transactionRepository.Selected.Currency);
        }

        public async void Save() {
            if (IsEdit) {
                await TransactionLogic.UpdateTransaction(_transactionRepository.Selected);
            }
            else {
                await TransactionLogic.SaveTransaction(_transactionRepository.Selected, RefreshRealtedList);
            }

            ((Frame) Window.Current.Content).GoBack();
        }

        public async void Cancel() {
            if (IsEdit) {
                await AccountLogic.AddTransactionAmount(_transactionRepository.Selected);
            }

            ((Frame) Window.Current.Content).GoBack();
        }
    }
}