namespace MoneyFox.Domain.Exceptions;

public class NoPageForViewModelRegisteredException(Type type) : Exception(message: $"No Page for ViewModel of type {type} registered");
