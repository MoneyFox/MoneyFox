using System;

namespace MoneyFox.DataAccess.Infrastructure
{
    /// <summary>
    ///     Implements base methods for disposeable objects
    /// </summary>
    public class Disposable : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        ///     Dispose the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Default deconstructor
        /// </summary>
        ~Disposable()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                DisposeCore();
            }

            isDisposed = true;
        }

        /// <summary>
        ///     Overide this to dispose custom objects
        /// </summary>
        protected virtual void DisposeCore()
        {
        }
    }
}