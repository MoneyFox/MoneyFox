using System;
using System.Linq;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel(IAccountRepository accountRepository,
            IPaymentRepository paymentRepository, 
            IDialogService dialogService, IEndOfMonthManager endOfMonthManager, ISettingsManager settingsManager)
        {
            AccountListViewModel = new AccountListViewModel(accountRepository, paymentRepository, dialogService, endOfMonthManager, settingsManager);
            ViewActionViewModel = new AccountListViewActionViewModel(accountRepository);
        }

        public IAccountListViewModel AccountListViewModel { get; }

        public IViewActionViewModel ViewActionViewModel { get; }

        //Used in Android and IOS.
        public void ShowMenuAndFirstDetail()
        {
            ShowViewModel<AccountListViewModel>();
            ShowViewModel<MenuViewModel>();
        }
    }
}