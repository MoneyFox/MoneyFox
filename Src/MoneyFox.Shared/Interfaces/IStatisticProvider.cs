using System;

namespace MoneyFox.Shared.Interfaces {
    /// <summary>
    ///     Defines the interface for selecting statistic data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStatisticProvider<out T> {
        /// <summary>
        ///     Returns a list of values for the givem Timerange.
        /// </summary>
        /// <param name="startDate">Startpoint form which to select data.</param>
        /// <param name="endDate">Endpoint form which to select data.</param>
        /// <returns>Statistic value for the given timeframe</returns>
        T GetValues(DateTime startDate, DateTime endDate);
    }
}