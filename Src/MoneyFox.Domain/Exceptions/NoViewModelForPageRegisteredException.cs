namespace MoneyFox.Domain.Exceptions;

public class NoViewModelForPageRegisteredException(Type type) : Exception(message: $"No ViewModel for page of type {type} registered");
