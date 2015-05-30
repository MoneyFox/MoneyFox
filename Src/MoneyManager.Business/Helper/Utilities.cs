using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using GalaSoft.MvvmLight.Views;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Business.Helper {
    public class Utilities {
        private readonly INavigationService _navigationService;

        public Utilities(INavigationService navigationService) {
            _navigationService = navigationService;
        }

        /// <summary>
        ///     Get the version of the MoneyManager.WindowsPhone dll
        /// </summary>
        /// <returns>version string</returns>
        public static string GetVersion() {
            var version = Package.Current.Id.Version;

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}.{1}.{2}.{3}",
                version.Major,
                version.Minor,
                version.Build,
                version.Revision);
        }

        /// <summary>
        /// Shows a Dialog to confirm that the selected item really shall be deleted.
        /// </summary>
        /// <returns>True or False if the user confirmed the deletion.</returns>
        public static async Task<bool> IsDeletionConfirmed() {
            var dialog = new MessageDialog(Translation.GetTranslation("DeleteEntryQuestionMessage"),
                Translation.GetTranslation("DeleteQuestionTitle"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));
            dialog.DefaultCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();

            return result.Label == Translation.GetTranslation("YesLabel");
        }

        /// <summary>
        ///     Returns the last day of the month
        /// </summary>
        /// <returns>Last day of the month</returns>
        public static DateTime GetEndOfMonth() {
            DateTime today = DateTime.Today;
            return new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        }

        /// <summary>
        ///     Will round all values of the passed statistic item list
        /// </summary>
        /// <param name="items">List of statistic items.</param>
        public static void RoundStatisticItems(List<StatisticItem> items) {
            foreach (StatisticItem item in items) {
                item.Value = Math.Round(item.Value, 2, MidpointRounding.AwayFromZero);
            }
        }
    }
}