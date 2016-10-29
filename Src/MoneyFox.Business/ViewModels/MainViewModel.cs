namespace MoneyFox.Business.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        //Used in Android and IOS.
        public void ShowMenuAndFirstDetail()
        {
            ShowViewModel<AccountListViewModel>();
            ShowViewModel<MenuViewModel>();
        }
    }
}