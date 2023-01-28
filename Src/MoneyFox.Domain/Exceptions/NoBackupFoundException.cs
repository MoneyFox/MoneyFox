namespace MoneyFox.Domain.Exceptions;

public class NoBackupFoundException : Exception
{
    public NoBackupFoundException() : base("No backup with was found") { }
}
