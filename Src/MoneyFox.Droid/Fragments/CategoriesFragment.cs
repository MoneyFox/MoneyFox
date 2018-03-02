using Android.OS;
using Android.Runtime;
using Android.Views;
using MoneyFox.Business.ViewModels;
using MoneyFox.Views;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using Xamarin.Forms.Platform.Android;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.CategoriesFragment")]
    public class CategoriesFragment : MvxFragment<CategoryListViewModel> 
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            Activity.Title = Strings.CategoriesLabel;

            var fragment = new CategoryListPage { BindingContext = ViewModel }.CreateSupportFragment(Activity);
            FragmentManager.BeginTransaction()
                           .Replace(Resource.Id.content_frame, fragment)
                           .Commit();

            //view.FindViewById<FloatingActionButton>(Resource.Id.fab_create_category).Click += (s, e) =>
            //{
            //    ViewModel.CreateNewCategoryCommand.Execute();
            //};

            return view;
        }
    }
}