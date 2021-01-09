using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using System;
using System.Threading.Tasks;

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
        }

        public RelayCommand ShowFilterDialogCommand => new RelayCommand(async () => await ShowFilterDialogAsync());

        private async Task ShowFilterDialogAsync() => await new DateSelectionPopup().ShowAsync();
    }
}
