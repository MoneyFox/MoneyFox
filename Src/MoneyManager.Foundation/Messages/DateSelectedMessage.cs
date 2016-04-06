using System;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Messenger;

namespace MoneyManager.Foundation.Messages
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
        public DateTime StartDate { get; private set; }

        /// <summary>
        ///     The selected end date
        /// </summary>
        public DateTime EndDate { get; private set; }

    }
}
