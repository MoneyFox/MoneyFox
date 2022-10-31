namespace MoneyFox.Core.ApplicationCore.UseCases.DbBackup;

public class UserAccountDto
{
    public UserAccountDto(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public string Name { get; }

    public string Email { get; }
}
