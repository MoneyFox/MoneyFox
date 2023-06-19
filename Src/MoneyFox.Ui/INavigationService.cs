namespace MoneyFox.Ui;

public interface INavigationService
{
    Task NavigateToAsync<T>() where T : ContentPage;

    Task NavigateToAsync<T>(string parameterName, string queryParameter) where T : ContentPage;

    Task NavigateBackAsync();

    Task NavigateBackAsync(string parameterName, string queryParameter);

    Task OpenModalAsync<T>() where T : ContentPage;

    Task GoBackFromModalAsync();
}
