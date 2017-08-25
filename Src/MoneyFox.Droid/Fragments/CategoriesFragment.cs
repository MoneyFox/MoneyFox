using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Shared.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.CategoriesFragment")]
    public class CategoriesFragment : BaseFragment<CategoryListViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_category_list;
        protected override string Title => Strings.CategoriesLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            view.FindViewById<FloatingActionButton>(Resource.Id.fab_create_category).Click += (s, e) =>
            {
                ViewModel.CreateNewCategoryCommand.Execute();
            };

            return view;
        }

        public override async void OnStart()
        {
            base.OnStart();
            await ViewModel.LoadedCommand.ExecuteAsync();
        }
    }
}