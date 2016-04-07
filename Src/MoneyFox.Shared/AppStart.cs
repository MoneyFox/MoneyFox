using MoneyFox.Shared.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            ShowViewModel<MainViewModel>();
        }
    }
}