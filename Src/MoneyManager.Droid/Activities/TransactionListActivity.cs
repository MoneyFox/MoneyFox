using Android.App;
using Android.OS;
using Android.Views;
using MoneyManager.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "TransactionListActivity")]
    public class TransactionListActivity : MvxActivity
    {
        public new PaymentListViewModel ViewModel
        {
            get { return (PaymentListViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        /// <summary>
        ///     Raises the create event.
        /// </summary>
        /// <param name="bundle">Saved instance state.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ViewModel.LoadedCommand.Execute();

            SetContentView(Resource.Layout.TransactionListLayout);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.Title = ViewModel.Title;
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