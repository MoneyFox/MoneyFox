using System;

namespace MoneyFox.BusinessLogic
{
    /// <summary>
    ///     Result of the customer operation
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        private OperationResult(bool success, string message = "") {
            Success = success;
            Message = message; 
        }

        /// <summary>
        ///     Indicates if the operation succeeded.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        ///     Result Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Creates a failed OperationResult
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>Operation Result</returns>
        public static OperationResult Failed(Exception ex) {
            return new OperationResult(false, ex?.ToString() ?? "");
        }

        /// <summary>
        ///     Creates a failed OperationResult
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Operation Result</returns>
        public static OperationResult Failed(string message = "") {
            return new OperationResult(false, message);
        }

        /// <summary>
        ///    Creates a succeeded OperationResult
        /// </summary>
        /// <returns>Operation Result</returns>
        public static OperationResult Succeeded() {
            return new OperationResult(true);
        }
    }
}
