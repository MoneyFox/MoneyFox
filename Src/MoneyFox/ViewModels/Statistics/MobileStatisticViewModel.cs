using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Common;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using SkiaSharp;
using System;

namespace MoneyFox.ViewModels.Statistics
{
    public abstract class MobileStatisticViewModel : StatisticViewModel
    {
        protected MobileStatisticViewModel(IMediator mediator)
            : base(mediator)
        {
        }

        protected MobileStatisticViewModel(DateTime startDate, DateTime endDate, IMediator mediator)
            : base(startDate, endDate, mediator)
        {
            BackgroundColor = SKColor.Parse(ResourceHelper.GetCurrentBackgroundColor().ToHex());
        }

        public RelayCommand ShowFilterDialogCommand => new RelayCommand(async() => await ShowFilterDialog());

        protected SKColor BackgroundColor { get; }

        private async void ShowFilterDialog()
        {
            await new FilterPopup().ShowAsync();
        }
    }
}
