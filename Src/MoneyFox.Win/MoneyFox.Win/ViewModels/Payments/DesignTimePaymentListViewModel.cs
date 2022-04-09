namespace MoneyFox.Win.ViewModels.Payments;

using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using DesignTime;
using Interfaces;
using Microsoft.UI.Xaml.Data;

public class DesignTimePaymentListViewModel : IPaymentListViewModel
{
    public RelayCommand<PaymentViewModel> EditPaymentCommand { get; } = null!;

    public RelayCommand<PaymentViewModel> DeletePaymentCommand { get; } = null!;
    public IBalanceViewModel BalanceViewModel => new DesignTimeBalanceViewViewModel();

    public IPaymentListViewActionViewModel ViewActionViewModel { get; } = null!;

    public RelayCommand InitializeCommand { get; } = null!;

    public CollectionViewSource GroupedPayments
        => new()
        {
            IsSourceGrouped = true,
            Source = new List<PaymentViewModel>
            {
                new() { Amount = 123, Category = new() { Name = "Beer" }, Date = DateTime.Now },
                new() { Amount = 123, Category = new() { Name = "Beer" }, Date = DateTime.Now.AddMonths(-1) },
                new() { Amount = 123, Category = new() { Name = "Beer" }, Date = DateTime.Now.AddMonths(-1) }
            }
        };

    public string Title => "Sparkonto";

    public int AccountId { get; } = 13;

    public RelayCommand LoadDataCommand => throw new NotSupportedException();
}
