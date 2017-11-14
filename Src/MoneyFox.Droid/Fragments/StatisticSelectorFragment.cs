using Android.Runtime;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Views.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.StatisticSelectorFragment")]
    public class StatisticSelectorFragment : BaseFragment<StatisticSelectorViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_statistic_selector;
        protected override string Title => Strings.StatisticTitle;
    }
}