using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MoneyFox.Droid.Fragments;
using MoneyFox.Shared;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "PaymentListActivity",
        Name = "moneyfox.droid.activities.PaymentListActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class PaymentListActivity : MvxAppCompatActivity<PaymentListViewModel>
    {
        private readonly List<string> itemForCreationList = new List<string>
        {
            Strings.AddIncomeLabel,
            Strings.AddExpenseLabel,
            Strings.AddTransferLabel
        };

        private MvxExpandableListView PaymentExpandable;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_payment_list);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            FindViewById<FloatingActionButton>(Resource.Id.fab_create_payment).Click += (s, e) =>
            {
                var builder = new AlertDialog.Builder(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
                builder.SetTitle(Strings.ChooseLabel);
                builder.SetItems(itemForCreationList.ToArray(), OnSelectItemForCreation);
                builder.SetNegativeButton(Strings.CancelLabel, (d, t) => (d as Dialog).Dismiss());
                builder.Show();
            };

            PaymentExpandable = FindViewById<MvxExpandableListView>(Resource.Id.expandable_payment_list);
            PaymentExpandable.ExpandGroup(0);
            RegisterForContextMenu(PaymentExpandable);

            LoadBalancePanel();
            Title = ViewModel.Title;
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.expandable_payment_list)
            {
                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(0, 0, 0, Strings.EditLabel);
                menu.Add(0, 1, 1, Strings.DeleteLabel);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var selected = ViewModel.RelatedPayments[ExpandableListView
                .GetPackedPositionChild(((
                    ExpandableListView.ExpandableListContextMenuInfo) item.MenuInfo)
                    .PackedPosition)];

            switch (item.ItemId)
            {
                case 0:
                    ViewModel.EditCommand.Execute(selected);
                    return true;

                case 1:
                    ViewModel.DeletePaymentCommand.Execute(selected);
                    return true;

                default:
                    return false;
            }
        }

        public void OnSelectItemForCreation(object sender, DialogClickEventArgs args)
        {
            var selected = itemForCreationList[args.Which];

            if (selected == Strings.AddIncomeLabel)
            {
                ViewModel.GoToAddPaymentCommand.Execute(PaymentType.Income.ToString());
            }
            else if (selected == Strings.AddExpenseLabel)
            {
                ViewModel.GoToAddPaymentCommand.Execute(PaymentType.Expense.ToString());
            }
            else if (selected == Strings.AddTransferLabel)
            {
                ViewModel.GoToAddPaymentCommand.Execute(PaymentType.Transfer.ToString());
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