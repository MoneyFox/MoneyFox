using System;
using Xamarin;

namespace MoneyManager.Foundation {
    /// <summary>
    /// Helperclass for Insights
    /// </summary>
    public class InsightHelper {
        /// <summary>
        /// Reports the passed exception if Insights are initialized
        /// </summary>
        /// <param name="exception">Excpetion to report.</param>
        /// <param name="serverity">Serverity for the to report the exception.</param>
        public static void Report(Exception exception, ReportSeverity serverity  = ReportSeverity.Error) {
            if (Insights.IsInitialized) {
                Insights.Report(exception, serverity);
            }
        }
    }
}
