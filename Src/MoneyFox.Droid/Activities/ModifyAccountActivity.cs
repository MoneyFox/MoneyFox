using MoneyFox.Shared.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "ModifyAccountActivity",
        Name = "moneyfox.droid.activities.ModifyAccountActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class ModifyAccountActivity : MvxAppCompatActivity<ModifyAccountViewModel>
    {
        private EditText editTextCurrentBalance;

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

            editTextCurrentBalance = FindViewById<EditText>(Resource.Id.edit_text_current_balance);
            editTextCurrentBalance.FocusChange += EditTextCurrentBalanceOnFocusChange;
            editTextCurrentBalance.Text = ViewModel.AmountString;

            Title = ViewModel.Title;
        }

        private void EditTextCurrentBalanceOnFocusChange(object sender, View.FocusChangeEventArgs focusChangeEventArgs)
        {
            if (!focusChangeEventArgs.HasFocus)
            {
                ViewModel.AmountString = editTextCurrentBalance.Text;
                editTextCurrentBalance.Text = ViewModel.AmountString;
            }
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
                    ViewModel.AmountString = editTextCurrentBalance.Text;
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