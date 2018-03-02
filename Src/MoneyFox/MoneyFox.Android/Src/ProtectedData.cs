using Android.App;
using Android.Content;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Droid.Src
{
    /// <inheritdoc />
    public class ProtectedData : IProtectedData
    {
        private readonly ISharedPreferences preferences;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ProtectedData()
        {
            preferences = Application.Context.GetSharedPreferences(Application.Context.PackageName + ".SecureStorage",
                FileCreationMode.Private);
        }

        /// <inheritdoc />
        public void Protect(string key, string value)
        {
            var editor = preferences.Edit();
            editor.PutString(key, value);
            editor.Commit();
        }

        /// <inheritdoc />
        public string Unprotect(string key)
        {
            try
            {
                return preferences.GetString(key, null);
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            if (preferences.Contains(key))
            {
                var editor = preferences.Edit();
                editor.Remove(key);
                editor.Commit();
            }
        }
    }
}