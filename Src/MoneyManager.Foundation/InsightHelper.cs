using System;
using Xamarin;

namespace MoneyManager.Foundation {
    /// <summary>
    /// Helperclass for Insights
    /// </summary>
    public class InsightHelper {
        /// <summary>
        /// Reports the passed exception if Insights are initialized with severity Error.
        /// </summary>
        /// <param name="exception">Excpetion to report.</param>
        public static void Report(Exception exception) {
            Report(exception, Insights.Severity.Error);
        }

        /// <summary>
        /// Reports the passed exception if Insights are initialized
        /// </summary>
        /// <param name="exception">Excpetion to report.</param>
        /// <param name="severity">Serverity for the to report the exception.</param>
        public static void Report(Exception exception, Insights.Severity severity) {
            if (Insights.IsInitialized) {
                Insights.Report(exception, severity);
            }
        }
    }
}
