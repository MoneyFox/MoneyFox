namespace MoneyFox.Win;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Pages;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        RootFrame = ShellFrame;
        RootFrame.Navigate(sourcePageType: typeof(ShellPage), parameter: null);
    }

    // This is a temporary fix until WinUI Dialogs are fixed
    public static Frame RootFrame { get; private set; }
}
