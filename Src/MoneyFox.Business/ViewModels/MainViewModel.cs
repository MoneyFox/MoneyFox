namespace MoneyFox.Business.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public void ShowMenuAndFirstDetail()
        {
            ShowViewModel<MenuViewModel>();
            ShowViewModel<AccountListViewModel>();
        }
    }
}