using System;
using System.Threading.Tasks;
using MoneyFox.Business.Extensions;
using MoneyFox.Business.Messages;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Plugin.Messenger;
using SkiaSharp;

namespace MoneyFox.Business.ViewModels.Statistic
{
    /// <summary>
    ///     Reprsents the statistic view.
    /// </summary>
    public abstract class StatisticViewModel : BaseViewModel
    {
        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;

        private DateTime startDate;
        private DateTime endDate;

        protected SKColor BackgroundColor;

        /// <summary>
        ///     Creates a StatisticViewModel Object and passes the first and last day of the current month
        ///     as a start and end date.
        /// </summary>
        protected StatisticViewModel(IMvxMessenger messenger, ISettingsManager settingsManager)
            : this(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth(), messenger, settingsManager)
        {
        }

        /// <summary>
        ///     Creates a Statistic ViewModel with custom start and end date
        /// </summary>
        /// <param name="startDate">Start date to select data from.</param>
        /// <param name="endDate">End date to select date from.</param>
        /// <param name="messenger">Messenger Instance</param>
        /// <param name="settingsManager">Instance of a SettingsManager</param>
        protected StatisticViewModel(DateTime startDate, DateTime endDate, IMvxMessenger messenger, ISettingsManager settingsManager)
        {
            StartDate = startDate;
            EndDate = endDate;

            BackgroundColor = settingsManager.Theme == AppTheme.Dark
                ? new SKColor(0, 0, 0)
                : SKColor.Parse("#EFF2F5");

            token = messenger.SubscribeOnMainThread<DateSelectedMessage>(message =>
            {
                StartDate = message.StartDate;
                EndDate = message.EndDate;
                Load();
            });
        }

        /// <inheritdoc />
        public override async Task Initialize()
        {
            await Load();
        }

        /// <summary>
        ///     Startdate for a custom statistic
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
        ///     Enddate for a custom statistic
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
        public string Title => Strings.StatisticsRangeTitle + " " + StartDate.ToString("d") +
                               " - " +
                               EndDate.ToString("d");

        protected abstract Task Load();
    }
}