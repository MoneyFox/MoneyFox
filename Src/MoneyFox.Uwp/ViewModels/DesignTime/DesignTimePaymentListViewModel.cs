using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Ui.Shared.ViewModels.Categories;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Xamarin.Forms;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    [SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "Not needed in design time")]
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public IBalanceViewModel BalanceViewModel => new DesignTimeBalanceViewViewModel();

        public IPaymentListViewActionViewModel ViewActionViewModel { get; } = null!;

        public AsyncCommand InitializeCommand { get; } = null!;

        public Command<PaymentViewModel> EditPaymentCommand { get; } = null!;

        public Command<PaymentViewModel> DeletePaymentCommand { get; } = null!;

        public ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source
                                                                                                        => new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>
        {
            new DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>("Januar 1992")
            {
                new DateListGroupCollection<PaymentViewModel>("31.1.1992")
                {
                    new PaymentViewModel { Amount = 123, Category = new CategoryViewModel { Name = "Beer" } },
                    new PaymentViewModel { Amount = 123, Category = new CategoryViewModel { Name = "Beer" } }
                }
            }
        };

        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList { get; } = null!;

        public string Title => "Sparkonto";

        public int AccountId { get; } = 13;

        public bool IsPaymentsEmpty { get; }

        public AsyncCommand LoadDataCommand => throw new NotImplementedException();
    }
}
