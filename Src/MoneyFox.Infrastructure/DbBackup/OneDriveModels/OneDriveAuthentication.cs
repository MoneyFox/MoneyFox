namespace MoneyFox.Infrastructure.DbBackup.OneDriveModels;

public class OneDriveAuthentication
{
    public OneDriveAuthentication(string accessToken, string tokenType)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
    }

    public string AccessToken { get; }
    public string TokenType { get; }
}
