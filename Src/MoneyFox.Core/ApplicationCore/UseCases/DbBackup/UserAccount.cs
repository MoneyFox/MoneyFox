namespace MoneyFox.Core.DbBackup
{

    public class UserAccount
    {
        public UserAccount(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }

        public string Email { get; }

        public UserAccount GetUserAccount()
        {
            return this;
        }
    }

}
