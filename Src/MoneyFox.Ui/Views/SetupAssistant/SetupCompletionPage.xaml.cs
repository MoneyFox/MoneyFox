﻿namespace MoneyFox.Ui.Views.SetupAssistant;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SetupCompletionPage : ContentPage
{
    public SetupCompletionPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupCompletionViewModel>();
    }
}