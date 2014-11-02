using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.Model;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectCurrencyViewModel : ViewModelBase
    {
        private static AddTransactionViewModel AddTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public ObservableCollection<Country> AllCountries { get; set; }

        private Country selecteCountry;
        public Country SelectedCountry
        {
            get { return selecteCountry; }
            set
            {
                if (value == null) return;

                selecteCountry = value;
                string current = CultureInfo.CurrentCulture.Name.Split('-')[1];
                if (selecteCountry.ID != current)
                {
                    AddTransactionView.IsExchangeModeActive = true;
                    var culture = new CultureInfo(selecteCountry.ID);
                    AddTransactionView.SelectedTransaction.CurrencyCulture = culture.Name;
                }
                ((Frame)Window.Current.Content).GoBack();
            }
        }

        public async Task LoadCountries()
        {
            AllCountries = new ObservableCollection<Country>(await CurrencyLogic.GetSupportedCountries());
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value.ToUpper();
                Search();
            }
        }

        public async void Search()
        {
            if (SearchText != String.Empty)
            {
                AllCountries =
                    new ObservableCollection<Country>(
                        AllCountries
                            .Where(x => x.ID.Contains(searchText) || x.CurrencyID.Contains(SearchText))
                            .ToList());
            }
            else
            {
                await LoadCountries();

            }
        }

    }
}
