using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.AccountListFragment")]
    public class AccountListFragment : BaseFragment<AccountListViewModel>
    {
        private readonly List<string> itemsForCreationList = new List<string>
        {
            Strings.AddAccountLabel,
            Strings.AddIncomeLabel,
            Strings.AddExpenseLabel,
            Strings.AddTransferLabel
        };

        private View view;
        protected override int FragmentId => Resource.Layout.fragment_account_list;
        protected override string Title => Strings.AccountsLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            LoadBalancePanel();

            var list = view.FindViewById<ListView>(Resource.Id.account_list);
            RegisterForContextMenu(list);

            var button = view.FindViewById<FloatingActionButton>(Resource.Id.fab_create_item);
            button.Click += (s, e) =>
            {
                var builder = new AlertDialog.Builder(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
                builder.SetTitle(Strings.ChooseLabel);
                builder.SetItems(GetItemArrayForCreationList(), OnSelectItemForCreation);
                builder.SetNegativeButton(Strings.CancelLabel, (d, t) => (d as Dialog)?.Dismiss());
                builder.Show();
            };

            return view;
        }

        public override void OnStart()
        {
            ViewModel.LoadedCommand.Execute();
            base.OnStart();
        }

        private string[] GetItemArrayForCreationList()
        {
            if (ViewModel.AllAccounts?.Count > 1)
            {
                return itemsForCreationList.ToArray();
            }
            var returnArray = new string[itemsForCreationList.Count - 1];
            itemsForCreationList.CopyTo(0, returnArray, 0, itemsForCreationList.Count - 1);

            return returnArray;
        }

        public void OnSelectItemForCreation(object sender, DialogClickEventArgs args)
        {
            var selected = itemsForCreationList[args.Which];
            var mainview = Mvx.Resolve<MainViewModel>();

            if (selected == Strings.AddAccountLabel)
            {
                mainview.GoToAddAccountCommand.Execute();
            }
            else if (selected == Strings.AddIncomeLabel)
            {
                mainview.GoToAddPaymentCommand.Execute(PaymentType.Income.ToString());
            }
            else if (selected == Strings.AddExpenseLabel)
            {
                mainview.GoToAddPaymentCommand.Execute(PaymentType.Expense.ToString());
            }
            else if (selected == Strings.AddTransferLabel)
            {
                mainview.GoToAddPaymentCommand.Execute(PaymentType.Transfer.ToString());
            }
        }

        private void LoadBalancePanel()
        {
            var fragment = new BalanceFragment
            {
                ViewModel = (BalanceViewModel) ViewModel.BalanceViewModel
            };

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.account_list_balance_frame, fragment)
                .Commit();
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.account_list)
            {
                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(0, 0, 0, Strings.EditLabel);
                menu.Add(0, 1, 1, Strings.DeleteLabel);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var selected = ViewModel.AllAccounts[((AdapterView.AdapterContextMenuInfo) item.MenuInfo).Position];

            switch (item.ItemId)
            {
                case 0:
                    ViewModel.EditAccountCommand.Execute(selected);
                    return true;

                case 1:
                    ViewModel.DeleteAccountCommand.Execute(selected);
                    return true;

                default:
                    return false;
            }
        }
    }
}