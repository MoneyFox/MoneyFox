namespace MoneyFox.Domain.Exceptions;

public class ViewModelForPageNotFoundException(Type type) : Exception(message: $"ViewModel for page {type} not found");
