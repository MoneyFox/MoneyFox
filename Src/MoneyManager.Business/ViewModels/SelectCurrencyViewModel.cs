#region

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Manager;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectCurrencyViewModel : ViewModelBase
    {
        private readonly CurrencyManager _currencyManager;
        private string _searchText;

        public SelectCurrencyViewModel(CurrencyManager currencyManager)
        {
            _currencyManager = currencyManager;
        }

        public ObservableCollection<Country> AllCountries { get; set; }
        public InvocationType InvocationType { get; set; }

        public Country SelectedCountry
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                SetValue(value);
                ((Frame) Window.Current.Content).GoBack();
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value.ToUpper();
                Search();
            }
        }

        private void SetValue(Country value)
        {
            switch (InvocationType)
            {
                case InvocationType.Setting:
                    ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency = value.CurrencyID;
                    break;

                case InvocationType.Transaction:
                    ServiceLocator.Current.GetInstance<AddTransactionViewModel>().SetCurrency(value.CurrencyID);
                    break;

                case InvocationType.Account:
                    ServiceLocator.Current.GetInstance<AddAccountViewModel>().SetCurrency(value.CurrencyID);
                    break;
            }
        }

        public async Task LoadCountries()
        {
            AllCountries = new ObservableCollection<Country>(await _currencyManager.GetSupportedCountries());
        }

        public async void Search()
        {
            if (SearchText != string.Empty)
            {
                AllCountries =
                    new ObservableCollection<Country>(
                        AllCountries
                            .Where(x => x.ID.Contains(_searchText) || x.CurrencyID.Contains(SearchText))
                            .ToList());
            } else
            {
                await LoadCountries();
            }
        }
    }
}