namespace MoneyFox.Core.DbBackup
{
    public class UserAccount
    {
        public UserAccount(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public UserAccount GetUserAccount()
        {
            return this;
        }
    }
}
