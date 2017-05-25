namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of the MainView
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        public void ShowMenuAndFirstDetail()
        {
            ShowViewModel<MenuViewModel>();
            ShowViewModel<AccountListViewModel>();
        }
    }
}