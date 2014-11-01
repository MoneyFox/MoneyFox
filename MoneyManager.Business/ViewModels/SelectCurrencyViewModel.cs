using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.Model;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectCurrencyViewModel : ViewModelBase
    {
        public ObservableCollection<Country> AllCountries { get; set; }

        public Country SelectedCurrency { get; set; }
        
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
