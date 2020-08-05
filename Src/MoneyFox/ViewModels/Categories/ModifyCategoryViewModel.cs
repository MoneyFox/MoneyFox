using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Categories
{
    public class ModifyCategoryViewModel : BaseViewModel
    {
        public RelayCommand SaveCommand => new RelayCommand(async () => await SaveCategoryBaseAsync());

        private async Task SaveCategoryBaseAsync()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
