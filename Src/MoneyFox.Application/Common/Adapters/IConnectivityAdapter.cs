﻿namespace MoneyFox.Application.Common.Adapters
{
    /// <summary>
    ///     Provides access to the connectivity state.
    /// </summary>
    public interface IConnectivityAdapter
    {
        /// <summary>
        ///     returns if the device is connected to the internet.
        /// </summary>
        bool IsConnected { get; }
    }
}