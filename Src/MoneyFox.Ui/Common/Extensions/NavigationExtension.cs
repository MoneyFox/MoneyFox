namespace MoneyFox.Ui.Common.Extensions;

using Core.ApplicationCore.Domain.Exceptions;
using Serilog;

public static class NavigationExtension
{
    public static Task GoToModalAsync(this Shell shell, string route)
    {
        try
        {
            if (!(Routing.GetOrCreateContent(route) is Page page))
            {
                return Task.CompletedTask;
            }

            return shell.Navigation.PushModalAsync(page);
        }
        catch (Exception ex)
        {
            var exception = new NavigationException(message: $"Navigation to route {route} failed. ", innerException: ex);
            Log.Error(exception: exception, messageTemplate: "Error during navigation");

            throw exception;
        }
    }
}
