namespace MoneyFox.Business.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public void ShowMenuAndFirstDetail()
        {
            ShowViewModel<AccountListViewModel>();
            ShowViewModel<MenuViewModel>();
        }
    }
}