using Android.App;
using Android.Content;
using MoneyFox.Foundation.Interfaces;
using Plugin.SecureStorage;

namespace MoneyFox.Droid
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
            CrossSecureStorage.Current.SetValue(key, value);
        }

        /// <inheritdoc />
        public string Unprotect(string key)
        {
            try
            {
                if (preferences.Contains(key))
                {
                    var value = preferences.GetString(key, null);
                    if (value != null)
                    {
                        Protect(key, value);
                        var editor = preferences.Edit();
                        editor.Remove(key);
                        editor.Commit();
                    }
                }
                return CrossSecureStorage.Current.GetValue(key);
            } 
            catch
            {
                return null;
            }
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            CrossSecureStorage.Current.DeleteKey(key);
        }
    }
}