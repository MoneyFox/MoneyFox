using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Clans.Fab;
using MoneyFox.Business.ViewModels;
using MoneyFox.Droid.Dialogs;
using MoneyFox.Droid.Fragments;
using MvvmCross.Droid.Support.V7.AppCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "PaymentListActivity",
        Name = "moneyfox.droid.activities.PaymentListActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class PaymentListActivity : MvxAppCompatActivity<PaymentListViewModel>
    {
        private MvxRecyclerView innerPaymentlist;

        /// <inheritdoc />
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_payment_list);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            FindViewById<FloatingActionMenu>(Resource.Id.fab_menu_add_element).SetClosedOnTouchOutside(true);

            innerPaymentlist = FindViewById<MvxRecyclerView>(Resource.Id.inner_payment_list);
            //RegisterForContextMenu(innerPaymentlist);

            LoadBalancePanel();
            Title = ViewModel.Title;
        }

        /// <inheritdoc />
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.expandable_payment_list)
            {
                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(0, 0, 0, Strings.EditLabel);
                menu.Add(0, 1, 1, Strings.DeleteLabel);
            }
        }

        /// <inheritdoc />
        public override bool OnContextItemSelected(IMenuItem item)
        {
            var selected = (PaymentViewModel)
                innerPaymentlist.Adapter.GetItem(((AdapterView.AdapterContextMenuInfo) item.MenuInfo).Position);

            switch (item.ItemId)
            {
                case 0:
                    ViewModel.EditPaymentCommand.Execute(selected);
                    return true;

                case 1:
                    ViewModel.DeletePaymentCommand.Execute(selected);
                    return true;

                default:
                    return false;
            }
        }

        private void LoadBalancePanel()
        {
            var fragment = new BalanceFragment
            {
                ViewModel = (PaymentListBalanceViewModel) ViewModel.BalanceViewModel
            };

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.payment_list_balance_frame, fragment)
                .Commit();
        }

        /// <inheritdoc />
        protected override async void OnResume()
        {
            base.OnResume();
            await ViewModel.LoadCommand.ExecuteAsync();
        }

        /// <summary>
        ///     Initialize the contents of the Activity's standard options menu.
        /// </summary>
        /// <param name="menu">The options menu in which you place your items.</param>
        /// <returns>To be added.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_filter, menu);
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

                case Resource.Id.action_set_filter:
                    ShowFilterDialog();
                    return true;

                default:
                    return false;
            }
        }

        private void ShowFilterDialog()
        {
            var dialog = new SelectFilterDialog();
            dialog.Show(SupportFragmentManager.BeginTransaction(), Strings.SelectDateTitle);
        }
    }
}