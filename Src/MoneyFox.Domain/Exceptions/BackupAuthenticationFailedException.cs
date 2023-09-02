namespace MoneyFox.Domain.Exceptions;

public class BackupAuthenticationFailedException : Exception
{
    public BackupAuthenticationFailedException() : base("Backup Authentication Failed") { }

    public BackupAuthenticationFailedException(string message) : base(message) { }

    public BackupAuthenticationFailedException(Exception exception) : base(message: "Backup Authentication Failed", innerException: exception) { }
}
