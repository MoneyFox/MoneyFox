using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Views;

namespace MoneyFox.Controls
{
    public class BottomTabbedPage : MvxTabbedPage
    {

    }

    public class BottomTabbedPage<TViewModel> : BottomTabbedPage, IMvxPage<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public TViewModel ViewModel { get; set; }
    }
}
