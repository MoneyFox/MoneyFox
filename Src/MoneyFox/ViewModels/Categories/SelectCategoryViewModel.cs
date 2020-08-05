using GalaSoft.MvvmLight.Command;
using MoneyFox.Messages;

namespace MoneyFox.ViewModels.Categories
{
    public class SelectCategoryViewModel : CategoryListViewModel
    {
        public RelayCommand<CategoryViewModel> SelectCategoryCommand => new RelayCommand<CategoryViewModel>(async (c) =>
        {
            MessengerInstance.Send(new CategorySelectedMessage(c));
            await App.Current.MainPage.Navigation.PopModalAsync();
        });

    }
}
