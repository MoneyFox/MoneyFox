﻿using CommunityToolkit.Mvvm.Input;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    [SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "<Pending>")]
    public class DesignTimeBalanceViewViewModel : IBalanceViewModel
    {
        /// <inheritdoc />
        public decimal TotalBalance => 1784;

        /// <inheritdoc />
        public decimal EndOfMonthBalance => 9784;

        /// <inheritdoc />
        public AsyncRelayCommand UpdateBalanceCommand { get; } = null!;
    }
}