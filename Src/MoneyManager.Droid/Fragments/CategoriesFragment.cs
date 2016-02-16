using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using Android.Runtime;

namespace MoneyManager.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneymanager.droid.fragments.CategoriesFragment")]
    public class CategoriesFragment : BaseFragment<CategoryListViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_category_list;
    }
}

