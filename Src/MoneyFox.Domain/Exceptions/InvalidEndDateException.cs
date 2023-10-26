namespace MoneyFox.Domain.Exceptions;

public class InvalidEndDateException : Exception
{
    public InvalidEndDateException() : base("EndDate has to be today or in the future.") { }
}
