namespace MoneyFox.Core.Features.DbBackup;

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
