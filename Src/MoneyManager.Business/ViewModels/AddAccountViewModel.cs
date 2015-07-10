using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using MoneyManager.Business.Manager;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class AddAccountViewModel : ViewModelBase
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly CurrencyManager _currencyManager;
        private readonly SettingDataAccess _settings;

        public AddAccountViewModel(IRepository<Account> accountRepository, CurrencyManager currencyManager,
            SettingDataAccess settings)
        {
            _currencyManager = currencyManager;
            _settings = settings;
            _accountRepository = accountRepository;
        }

        public bool IsEdit { get; set; }

        public Account SelectedAccount
        {
            get { return _accountRepository.Selected; }
            set { _accountRepository.Selected = value; }
        }

        public string CurrentBalanceString
        {
            get { return CurrentBalanceWithoutExchange.ToString(); }
            set
            {
                double amount;
                if (double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentUICulture, out amount))
                {
                    CurrentBalanceWithoutExchange = amount;
                }
            }
        }

        public double CurrentBalanceWithoutExchange
        {
            get { return _accountRepository.Selected.CurrentBalanceWithoutExchange; }
            set
            {
                _accountRepository.Selected.CurrentBalanceWithoutExchange = value;
                CalculateNewAmount(value);
            }
        }

        public async void SetCurrency(string currency)
        {
            _accountRepository.Selected.Currency = currency;
            _accountRepository.Selected.IsExchangeModeActive = true;
            await LoadCurrencyRatio();
            CalculateNewAmount(CurrentBalanceWithoutExchange);
        }

        private void CalculateNewAmount(double value)
        {
            if (Math.Abs(_accountRepository.Selected.ExchangeRatio) < 0.5)
            {
                _accountRepository.Selected.ExchangeRatio = 1;
            }

            _accountRepository.Selected.CurrentBalance = _accountRepository.Selected.ExchangeRatio*value;
        }

        public async Task LoadCurrencyRatio()
        {
            _accountRepository.Selected.ExchangeRatio =
                await _currencyManager.GetCurrencyRatio(_settings.DefaultCurrency, _accountRepository.Selected.Currency);
        }

        public void Save()
        {
            _accountRepository.Save(_accountRepository.Selected);
            ((Frame) Window.Current.Content).GoBack();
        }

        public void Cancel()
        {
            ((Frame) Window.Current.Content).GoBack();
        }
    }
}