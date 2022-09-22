namespace MoneyFox.Views.Backup
{
    using CommunityToolkit.Mvvm.ComponentModel;

    internal class UserAccountViewModel : ObservableObject
    {
        private string name = string.Empty;

        private string email = string.Empty;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
    }
}
