namespace MoneyFox.Domain.Exceptions;

public class BackupException : Exception
{
    public BackupException(string message) : base(message) { }
}
