using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using PropertyChanged;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class AddAccountViewModel : ViewModelBase
    {
        #region Properties

        public Account SelectedAccount
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount; }
            set { ServiceLocator.Current.GetInstance<AccountDataAccess>().SelectedAccount = value; }
        }

        public SettingDataAccess Settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        public bool IsEdit { get; set; }

        #endregion Properties

        public double CurrentBalanceWithoutExchange
        {
            get { return SelectedAccount.CurrentBalanceWithoutExchange; }
            set
            {
                SelectedAccount.CurrentBalanceWithoutExchange = value;
                CalculateNewAmount(value);
            }
        }

        public async void SetCurrency(string currency)
        {
            SelectedAccount.Currency = currency;
            await LoadCurrencyRatio();
            SelectedAccount.IsExchangeModeActive = true;
            CalculateNewAmount(CurrentBalanceWithoutExchange);
        }

        private void CalculateNewAmount(double value)
        {
            if (SelectedAccount.ExchangeRatio == 0)
            {
                SelectedAccount.ExchangeRatio = 1;
            }

            SelectedAccount.CurrentBalance = SelectedAccount.ExchangeRatio * value;
        }

        public async Task LoadCurrencyRatio()
        {
            SelectedAccount.ExchangeRatio = await CurrencyLogic.GetCurrencyRatio(Settings.DefaultCurrency, SelectedAccount.Currency);
        }

        public void Save()
        {
            if (IsEdit)
            {
                ServiceLocator.Current.GetInstance<AccountDataAccess>().Update(SelectedAccount);
            }
            else
            {
                ServiceLocator.Current.GetInstance<AccountDataAccess>().Save(SelectedAccount);
            }
            ((Frame)Window.Current.Content).GoBack();
        }

        public void Cancel()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}