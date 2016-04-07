namespace MoneyFox.Core.Interfaces
{
    public interface IPasswordStorage
    {
        void SavePassword(string password);
        string LoadPassword();
        void RemovePassword();
        bool ValidatePassword(string passwordToValidate);
    }
}