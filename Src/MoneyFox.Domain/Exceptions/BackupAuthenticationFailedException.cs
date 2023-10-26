namespace MoneyFox.Domain.Exceptions;

public class BackupAuthenticationFailedException : Exception
{
    public BackupAuthenticationFailedException() : base("Backup Authentication Failed") { }

    public BackupAuthenticationFailedException(Exception exception) : base(message: "Backup Authentication Failed", innerException: exception) { }
}
