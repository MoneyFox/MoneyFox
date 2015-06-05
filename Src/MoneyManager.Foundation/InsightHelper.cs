using System;
using Microsoft.ApplicationInsights;

namespace MoneyManager.Foundation
{
    /// <summary>
    ///     Helper for Insights
    /// </summary>
    public static class InsightHelper
    {
        private static readonly TelemetryClient Telemetry = new TelemetryClient();

        /// <summary>
        ///     Reports the passed <paramref name="exception" /> if Insights are
        ///     initialized with severity Error.
        /// </summary>
        /// <param name="exception">Excpetion to report.</param>
        public static void Report(Exception exception)
        {
            if (Telemetry.IsEnabled())
            {
                Telemetry.TrackException(exception);
            }
        }
    }
}