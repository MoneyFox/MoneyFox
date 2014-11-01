using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.Model;

namespace MoneyManager.Business.ViewModels
{
    public class SelectCurrencyViewModel : ViewModelBase
    {
        public ObservableCollection<Country> AllCountries { get; set; }

        public Country SelectedCurrency { get; set; }

        public string SearchText { get; set; }
        
        public async Task LoadCountries()
        {
            AllCountries = new ObservableCollection<Country>(await CurrencyLogic.GetSupportedCountries());
        }
    }
}
