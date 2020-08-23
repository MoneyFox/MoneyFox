using MediatR;
using MoneyFox.Common;
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

        protected SKColor BackgroundColor { get; }
    }
}
