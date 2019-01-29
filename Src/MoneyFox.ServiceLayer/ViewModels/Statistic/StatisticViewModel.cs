using System;
using System.Globalization;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.Extensions;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Messages;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using SkiaSharp;

namespace MoneyFox.ServiceLayer.ViewModels.Statistic
{
    /// <summary>
    ///     Represents the statistic view.
    /// </summary>
    public abstract class StatisticViewModel : BaseNavigationViewModel
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
        protected StatisticViewModel(IMvxMessenger messenger, ISettingsFacade settingsManager, IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : this(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth(), messenger, settingsManager, logProvider, navigationService)
        {
        }

        /// <summary>
        ///     Creates a Statistic ViewModel with custom start and end date
        /// </summary>
        protected StatisticViewModel(DateTime startDate, 
                                     DateTime endDate,
                                     IMvxMessenger messenger,
                                     ISettingsFacade settingsFacade,
                                     IMvxLogProvider logProvider, 
                                     IMvxNavigationService navigationService) : base (logProvider, navigationService)
        {
            StartDate = startDate;
            EndDate = endDate;

            BackgroundColor = settingsFacade.Theme == AppTheme.Dark
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