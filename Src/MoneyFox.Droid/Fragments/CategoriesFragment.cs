using Android.OS;
using Android.Runtime;
using MoneyFox.Business.ViewModels;
using MoneyFox.Business.Views;
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
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Activity.Title = Strings.CategoriesLabel;

            HasOptionsMenu = true;

            var fragment = new CategoryListPage { BindingContext = ViewModel }.CreateSupportFragment(Activity);
            FragmentManager.BeginTransaction()
                           .Replace(Resource.Id.content_frame, fragment)
                           .Commit();
        }
    }
}