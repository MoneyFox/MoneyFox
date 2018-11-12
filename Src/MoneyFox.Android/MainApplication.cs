using System;
using System.IO;
using Android.App;
using Android.Runtime;
using MoneyFox.Foundation.Constants;

namespace MoneyFox.Droid
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            DataAccess.ApplicationContext.DbPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                             DatabaseConstants.DB_NAME);

            base.OnCreate();
        }
    }
}