using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;

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
