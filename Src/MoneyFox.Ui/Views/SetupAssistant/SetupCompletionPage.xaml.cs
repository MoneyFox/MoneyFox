namespace MoneyFox.Views.SetupAssistant;

using Ui;
using ViewModels.SetupAssistant;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SetupCompletionPage : ContentPage
{
    public SetupCompletionPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupCompletionViewModel>();
    }
}
