using System;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.Business.Messages
{
    public class DateSelectedMessage : MvxMessage
    {
        public DateSelectedMessage(object sender, DateTime startDate, DateTime endDate)
            : base(sender)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        ///     The selected start date
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        ///     The selected end date
        /// </summary>
        public DateTime EndDate { get; }
    }
}