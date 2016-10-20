using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "Money Fox",
        Theme = "@style/AppTheme",
        NoHistory = true)]
    public class LoginActivity : MvxAppCompatActivity<LoginViewModel>
    {
        private EditText editTextPassword;
        private Button loginButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_login);

            editTextPassword = FindViewById<EditText>(Resource.Id.edit_text_password);
            loginButton = FindViewById<Button>(Resource.Id.button_login);

            loginButton.Click += Login;
        }

        private void Login(object sender, EventArgs e)
        {
            if (!Mvx.Resolve<IPasswordStorage>().ValidatePassword(editTextPassword.Text))
            {
                Mvx.Resolve<IDialogService>().ShowMessage(Strings.PasswordWrongTitle, Strings.PasswordWrongMessage);
                return;
            }

            ViewModel.LoginNavigationCommand.Execute();
        }
    }
}