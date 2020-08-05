using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Resources;
using MoneyFox.Domain;
using MoneyFox.Extensions;
using MoneyFox.Groups;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.ViewModels.Categories;
using MoneyFox.Views.Payments;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Payments
{
    public class PaymentListViewModel : BaseViewModel
    {
        private AccountViewModel selectedAccount = new AccountViewModel { Name = "Food" };

        public void Init(int accountId)
        {
            Debug.Write($"Account Id passed {accountId}");
        }

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> Payments { get; set; } = new ObservableCollection<DateListGroupCollection<PaymentViewModel>>
        {
            new DateListGroupCollection<PaymentViewModel>("25.07.2020", string.Format(Strings.EarnedSpentTemplateLabel, 1234, 523))
            {
                new PaymentViewModel{ Date = DateTime.Now, Amount = 154, Type = PaymentType.Expense, Category = new CategoryViewModel { Name = "Food" }},
                new PaymentViewModel{ Date = DateTime.Now, Amount = 286, Type = PaymentType.Income },
                new PaymentViewModel{ Date = DateTime.Now, Amount = 286, Type = PaymentType.Transfer, TargetAccount = new AccountViewModel{ Name = "asdf" } },
            },
            new DateListGroupCollection<PaymentViewModel>("24.07.2020", string.Format(Strings.EarnedSpentTemplateLabel, 221, 2314))
            {
                new PaymentViewModel{ Date = DateTime.Now, Amount = 154, Type = PaymentType.Expense, IsRecurring = true, IsCleared = true },
                new PaymentViewModel{ Date = DateTime.Now, Amount = 286, Type = PaymentType.Income, Category = new CategoryViewModel { Name = "Food" }, IsCleared = true, Note="this is an example text"}
            }
        };

        public RelayCommand GoToAddPaymentCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddPaymentRoute));

        public RelayCommand<PaymentViewModel> GoToEditPaymentCommand
            => new RelayCommand<PaymentViewModel>(async (paymentViewModel)
                => await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditPaymentPage(paymentViewModel.Id)) { BarBackgroundColor = Color.Transparent }));
    }
}
