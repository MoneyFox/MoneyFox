using System;
using System.Globalization;
using System.Threading.Tasks;
using MoneyFox.Application;
using MoneyFox.Application.Resources;
using MoneyFox.BusinessLogic.Extensions;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Messages;
using SkiaSharp;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    /// <summary>
    ///     Represents the statistic view.
    /// </summary>
    public abstract class StatisticViewModel : BaseViewModel
    {
        private DateTime startDate;
        private DateTime endDate;

        protected SKColor BackgroundColor { get; }

        /// <summary>
        ///     Creates a StatisticViewModel Object and passes the first and last day of the current month
        ///     as a start and end date.
        /// </summary>
        protected StatisticViewModel(ISettingsFacade settingsManager)
            : this(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth(), settingsManager)
        {
        }

        /// <summary>
        ///     Creates a Statistic ViewModel with custom start and end date
        /// </summary>
        protected StatisticViewModel(DateTime startDate, 
                                     DateTime endDate,
                                     ISettingsFacade settingsFacade)
        {
            StartDate = startDate;
            EndDate = endDate;

            BackgroundColor = settingsFacade.Theme == AppTheme.Dark
                ? new SKColor(0, 0, 0)
                : SKColor.Parse("#EFF2F5");

            MessengerInstance.Register<DateSelectedMessage>(this, async message =>
            {
                StartDate = message.StartDate;
                EndDate = message.EndDate;
                await Load();
            });
        }

        public AsyncCommand LoadedCommand => new AsyncCommand(Load);
        
        /// <summary>
        ///     Start date for a custom statistic
        /// </summary>
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        ///     End date for a custom statistic
        /// </summary>
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value; 
                RaisePropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        ///     Returns the title for the CategoryViewModel view
        /// </summary>
        public string Title => Strings.StatisticsTimeRangeTitle + " " + StartDate.ToString("d", CultureInfo.InvariantCulture) +
                               " - " +
                               EndDate.ToString("d", CultureInfo.InvariantCulture);

        protected abstract Task Load();
    }
}