using System;
using System.Diagnostics;
using MvvmCross.Platform.Platform;

namespace MoneyFox.Business
{
    /// <summary>
    ///     Helper class to log debug messages of MvvmCross.
    /// </summary>
    public class DebugTrace : IMvxTrace
    {
        /// <summary>
        ///     Logs Trace messages.
        /// </summary>
        /// <param name="level">Trace level</param>
        /// <param name="tag">Tag</param>
        /// <param name="message">Message as function</param>
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Debug.WriteLine(tag + ":" + level + ":" + message());
        }

        /// <summary>
        ///     Logs Trace messages.
        /// </summary>
        /// <param name="level">Trace level</param>
        /// <param name="tag">Tag</param>
        /// <param name="message">Message</param>
        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Debug.WriteLine(tag + ":" + level + ":" + message);
        }

        /// <summary>
        ///     Logs Trace messages.
        /// </summary>
        /// <param name="level">Trace level</param>
        /// <param name="tag">Tag</param>
        /// <param name="message">Message as function</param>
        /// <param name="args">Arguments to log.</param>
        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            try
            {
                Debug.WriteLine(tag + ":" + level + ":" + message, args);
            } catch (FormatException)
            {
                Trace(MvxTraceLevel.Error, tag, "Exception during trace of {0} {1}", level, message);
            }
        }
    }
}