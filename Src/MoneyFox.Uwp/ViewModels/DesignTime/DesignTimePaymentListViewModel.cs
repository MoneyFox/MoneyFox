using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Ui.Shared.ViewModels.Categories;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Windows.UI.Xaml.Data;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    [SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "Not needed in design time")]
    public class DesignTimePaymentListViewModel : IPaymentListViewModel
    {
        public IBalanceViewModel BalanceViewModel => new DesignTimeBalanceViewViewModel();

        public IPaymentListViewActionViewModel ViewActionViewModel { get; } = null!;

        public RelayCommand InitializeCommand { get; } = null!;

        public RelayCommand<PaymentViewModel> EditPaymentCommand { get; } = null!;

        public RelayCommand<PaymentViewModel> DeletePaymentCommand { get; } = null!;

        public CollectionViewSource PaymentViewSource => new CollectionViewSource
        {
            IsSourceGrouped = true,
            Source = new DateListGroupCollection<PaymentViewModel>("Januar 1992")
            {
                new PaymentViewModel { Amount = 123, Category = new CategoryViewModel { Name = "Beer" }, Date = DateTime.Now },
                new PaymentViewModel { Amount = 123, Category = new CategoryViewModel { Name = "Beer" }, Date = DateTime.Now.AddMonths(-1) },
                new PaymentViewModel { Amount = 123, Category = new CategoryViewModel { Name = "Beer" }, Date = DateTime.Now.AddMonths(-1) }
            }
        };

        public ObservableCollection<DateListGroupCollection<PaymentViewModel>> DailyList { get; } = null!;

        public string Title => "Sparkonto";

        public int AccountId { get; } = 13;

        public RelayCommand LoadDataCommand => throw new NotImplementedException();
    }
}
