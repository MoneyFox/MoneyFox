namespace MoneyFox.Shared.Interfaces
{
    public interface IPasswordStorage
    {
        void SavePassword(string password);
        string LoadPassword();
        void RemovePassword();
        bool ValidatePassword(string passwordToValidate);
    }
}