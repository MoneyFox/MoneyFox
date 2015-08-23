using System;

namespace MoneyManager.Foundation
{
    /// <summary>
    ///     Helper for Insights
    /// </summary>
    public static class InsightHelper
    {
        //TODO: Use another loggin service.
        //private static readonly TelemetryClient Telemetry = new TelemetryClient();

        /// <summary>
        ///     Reports the passed exception if Insights are initialized
        /// </summary>
        /// <param name="exception">Excpetion to report.</param>
        public static void Report(Exception exception)
        {
            //if (Telemetry.IsEnabled())
            //{
            //    Telemetry.TrackException(exception);
            //}
        }
    }
}