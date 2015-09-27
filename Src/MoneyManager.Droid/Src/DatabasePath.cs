using System;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Droid
{
    public class DatabasePath : IDatabasePath
    {
        /// <summary>
        ///     returns the full db path.
        /// </summary>
        public string DbPath => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    }
}