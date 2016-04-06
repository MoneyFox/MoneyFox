using System;
using MoneyManager.Core.Extensions;
using MoneyManager.Foundation.Messages;
using MoneyManager.Localization;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public abstract class StatisticViewModel : BaseViewModel
    {
        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;

        /// <summary>
        ///     Creates a StatisticViewModel Object and passes the first and last day of the current month
        ///     as a start and end date.
        /// </summary>
        protected StatisticViewModel() 
            : this(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth())
        {}

        /// <summary>
        ///     Creates a Statistic ViewModel with custom start and end date
        /// </summary>
        /// <param name="startDate">Start date to select data from.</param>
        /// <param name="endDate">End date to select date from.</param>
        protected StatisticViewModel(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;

            token = MessageHub.Subscribe<DateSelectedMessage>(message =>
            {
                StartDate = message.StartDate;
                EndDate = message.EndDate;
            });
        }

        /// <summary>
        ///     Loads the data with the current start and end date.
        /// </summary>
        public MvxCommand LoadCommand => new MvxCommand(Load);

        protected abstract void Load();

        /// <summary>
        ///     Startdate for a custom statistic
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Enddate for a custom statistic
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     Returns the title for the category view
        /// </summary>
        public string Title => Strings.StatisticTitle + " " + StartDate.ToString("d") +
                               " - " +
                               EndDate.ToString("d");
    }
}