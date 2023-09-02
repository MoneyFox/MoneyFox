namespace MoneyFox.Domain.Exceptions;

public class BackupException : Exception
{
    public BackupException() { }

    public BackupException(string message) : base(message) { }
}
