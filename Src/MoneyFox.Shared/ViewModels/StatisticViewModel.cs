using System;
using MoneyFox.Business.Extensions;
using MoneyFox.Foundation.Resources;
using MoneyFox.Shared.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Shared.ViewModels
{
    public abstract class StatisticViewModel : BaseViewModel
    {
        //this token ensures that we will be notified when a message is sent.
        private readonly MvxSubscriptionToken token;
        private DateTime startDate;
        private DateTime endDate;

        /// <summary>
        ///     Creates a StatisticViewModel Object and passes the first and last day of the current month
        ///     as a start and end date.
        /// </summary>
        protected StatisticViewModel()
            : this(DateTime.Today.GetFirstDayOfMonth(), DateTime.Today.GetLastDayOfMonth())
        {
        }

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
                Load();
            });
        }

        /// <summary>
        ///     Loads the data with the current start and end date.
        /// </summary>
        public MvxCommand LoadCommand => new MvxCommand(Load);

        /// <summary>
        ///     Startdate for a custom statistic
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Enddate for a custom statistic
        /// </summary>
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value; 
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns the title for the CategoryViewModel view
        /// </summary>
        public string Title => Strings.StatisticTitle + " " + StartDate.ToString("d") +
                               " - " +
                               EndDate.ToString("d");

        protected abstract void Load();
    }
}