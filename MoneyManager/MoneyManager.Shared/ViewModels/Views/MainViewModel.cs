using GalaSoft.MvvmLight;
using MoneyManager.ViewModels.Data;

namespace MoneyManager.ViewModels.Views
{
    public class MainViewModel : ViewModelBase
    {
        public AccountViewModel AccountViewModel
        {
            get { return new ViewModelLocator().AccountViewModel; }
        }
    }
}