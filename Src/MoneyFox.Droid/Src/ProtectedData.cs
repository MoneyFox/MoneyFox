using Android.App;
using Android.Content;
using MoneyManager.Foundation.Interfaces;

namespace MoneyManager.Droid
{
    public class ProtectedData : IProtectedData
    {
        private readonly ISharedPreferences preferences;

        public ProtectedData()
        {
            preferences = Application.Context.GetSharedPreferences(Application.Context.PackageName + ".SecureStorage",
                FileCreationMode.Private);
        }

        public void Protect(string key, string value)
        {
            var editor = preferences.Edit();
            editor.PutString(key, value);
            editor.Commit();
        }

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