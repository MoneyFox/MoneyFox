#region

using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.Manager;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class AddAccountViewModel : ViewModelBase {
        private readonly CurrencyManager _currencyManager;

        public AddAccountViewModel(CurrencyManager currencyManager) {
            _currencyManager = currencyManager;
        }

        public string CurrentBalanceString {
            get { return CurrentBalanceWithoutExchange.ToString(); }
            set {
                double amount;
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentUICulture, out amount)) {
                    CurrentBalanceWithoutExchange = amount;
                }
            }
        }

        public double CurrentBalanceWithoutExchange {
            get { return SelectedAccount.CurrentBalanceWithoutExchange; }
            set {
                SelectedAccount.CurrentBalanceWithoutExchange = value;
                CalculateNewAmount(value);
            }
        }

        public async void SetCurrency(string currency) {
            SelectedAccount.Currency = currency;
            SelectedAccount.IsExchangeModeActive = true;
            await LoadCurrencyRatio();
            CalculateNewAmount(CurrentBalanceWithoutExchange);
        }

        private void CalculateNewAmount(double value) {
            if (Math.Abs(SelectedAccount.ExchangeRatio) < 0.5) {
                SelectedAccount.ExchangeRatio = 1;
            }

            SelectedAccount.CurrentBalance = SelectedAccount.ExchangeRatio*value;
        }

        public async Task LoadCurrencyRatio() {
            SelectedAccount.ExchangeRatio =
                await _currencyManager.GetCurrencyRatio(Settings.DefaultCurrency, SelectedAccount.Currency);
        }

        public void Save() {
            ServiceLocator.Current.GetInstance<AccountDataAccess>().Save(SelectedAccount);
            ((Frame) Window.Current.Content).GoBack();
        }

        public void Cancel() {
            ((Frame) Window.Current.Content).GoBack();
        }

        #region Properties

        public Account SelectedAccount {
            get { return ServiceLocator.Current.GetInstance<IAccountRepository>().Selected; }
            set { ServiceLocator.Current.GetInstance<IAccountRepository>().Selected = value; }
        }

        public SettingDataAccess Settings {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        public bool IsEdit { get; set; }

        #endregion Properties
    }
}