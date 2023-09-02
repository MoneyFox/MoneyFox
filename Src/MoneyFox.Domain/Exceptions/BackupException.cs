namespace MoneyFox.Domain.Exceptions;

public class BackupException : Exception
{
    /// <summary>
    ///     Creates an Backup Exception
    /// </summary>
    public BackupException() { }

    /// <summary>
    ///     Creates an Backup Exception
    /// </summary>
    /// <param name="message">Exception message to show to the user.</param>
    public BackupException(string message) : base(message) { }
}
