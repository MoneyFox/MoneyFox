using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.Extensions;
using MoneyManager.Core.StatisticProvider;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticViewModel : BaseViewModel
    {
        /// <summary>
        ///     Creates a StatisticViewModel Object.
        /// </summary>
        public StatisticViewModel()
        {
            StartDate = DateTime.Today.GetFirstDayOfMonth();
            EndDate = DateTime.Today.GetLastDayOfMonth();
        }

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