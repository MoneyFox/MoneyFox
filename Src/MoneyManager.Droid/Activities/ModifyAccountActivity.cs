using Android.App;
using Android.OS;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Android.Content.PM;
using Android.Support.V7.Widget;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "ModifyAccountActivity",
        Name = "moneymanager.droid.activities.ModifyAccountActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class ModifyAccountActivity : MvxCachingFragmentCompatActivity<ModifyAccountViewModel>
    {
        /// <summary>
        ///     Raises the create event.
        /// </summary>
        /// <param name="bundle">Saved instance state.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_modify_account);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        /// <summary>
        ///     Initialize the contents of the Activity's standard options menu.
        /// </summary>
        /// <param name="menu">The options menu in which you place your items.</param>
        /// <returns>To be added.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(ViewModel.IsEdit ? Resource.Menu.menu_modification : Resource.Menu.menu_add, menu);
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
                    ViewModel.SaveCommand.Execute(null);
                    return true;

                case Resource.Id.action_delete:
                    ViewModel.DeleteCommand.Execute(null);
                    return true;

                default:
                    return false;
            }
        }
    }
}