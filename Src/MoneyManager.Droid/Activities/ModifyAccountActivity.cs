using Android.App;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Droid.Views;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "ModifyAccountActivity")]
    public class ModifyAccountActivity : MvxActivity
    {
        public new ModifyAccountViewModel ViewModel
        {
            get { return (ModifyAccountViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        /// <summary>
        ///     Raises the create event.
        /// </summary>
        /// <param name="bundle">Saved instance state.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ModifyAccountLayout);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.Title = ViewModel.Title;
        }

        /// <summary>
        ///     Initialize the contents of the Activity's standard options menu.
        /// </summary>
        /// <param name="menu">The options menu in which you place your items.</param>
        /// <returns>To be added.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(ViewModel.IsEdit ? Resource.Menu.ModificationMenu : Resource.Menu.AddMenu, menu);

            return base.OnCreateOptionsMenu(menu);
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