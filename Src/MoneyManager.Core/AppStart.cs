using MoneyManager.Core.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            ShowViewModel<MainViewModel>();
        }
    }
}