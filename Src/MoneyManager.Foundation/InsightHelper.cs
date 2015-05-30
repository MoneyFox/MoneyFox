using System;
using Microsoft.ApplicationInsights;

namespace MoneyManager.Foundation
{
    /// <summary>
    ///     Helper for Insights
    /// </summary>
    public static class InsightHelper
    {
        private static readonly TelemetryClient TelemetryClient = new TelemetryClient();

        /// <summary>
        ///     Reports the passed <paramref name="exception" /> if Insights are
        ///     initialized with severity Error.
        /// </summary>
        /// <param name="exception">Excpetion to report.</param>
        public static void Report(Exception exception)
        {
            if (TelemetryClient.IsEnabled())
            {
                TelemetryClient.TrackException(exception);
            }
        }
    }
}