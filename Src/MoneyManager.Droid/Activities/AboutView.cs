using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Droid.Views;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "About", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AboutView : MvxActivity
    {
        public new AboutViewModel ViewModel
        {
            get { return (AboutViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AboutLayout);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            ActionBar.Title = Strings.AboutTitle;
        }

        /// <summary>
        ///     This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <param name="item">The menu item that was selected.</param>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                return true;
            }

            return false;
        }
    }
}