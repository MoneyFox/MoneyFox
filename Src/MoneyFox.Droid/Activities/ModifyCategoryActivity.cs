using Android.App;
using Android.OS;
using Android.Views;
using Android.Content.PM;
using MvvmCross.Droid.Support.V7.AppCompat;
using MoneyFox.Shared.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "ModifyCategoryActivity",
        Name = "moneyfox.droid.activities.ModifyCategoryActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class ModifyCategoryActivity : MvxAppCompatActivity<ModifyCategoryViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_modify_category);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            Title = ViewModel.Title;
        }

        /// <summary>
        ///     Initialize the contents of the Activity's standard options menu.
        /// </summary>
        /// <param name="menu">The options menu in which you place your items.</param>
        /// <returns>To be added.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(ViewModel.IsEdit ? Resource.Menu.menu_modification : Resource.Menu.menu_save, menu);
            return true;
        }

        /// <summary>
        ///     This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <param name="item">The menu item that was selected.</param>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.action_save:
                    ViewModel.SaveCommand.Execute();
                    return true;

                case Resource.Id.action_delete:
                    ViewModel.DeleteCommand.Execute();
                    return true;

                default:
                    return false;
            }
        }
    }
}