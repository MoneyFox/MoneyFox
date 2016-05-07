using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MoneyFox.Droid.Fragments;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "PaymentListActivity",
        Name = "moneyfox.droid.activities.PaymentListActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class PaymentListActivity : MvxAppCompatActivity<PaymentListViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_payment_list);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            LoadBalancePanel();
            Title = ViewModel.Title;
        }

        private void LoadBalancePanel()
        {
            var fragment = new BalanceFragment
            {
                ViewModel = (PaymentListBalanceViewModel)ViewModel.BalanceViewModel
            };

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.payment_list_balance_frame, fragment)
                .Commit();
        }

        protected override void OnResume()
        {
            base.OnResume();

            ViewModel.LoadCommand.Execute();
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

                default:
                    return false;
            }
        }
    }
}