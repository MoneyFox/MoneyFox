using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Views.Dialogs
{
    public sealed partial class LoadingDialog : ContentDialog
    {
        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LoadingDialog), new PropertyMetadata(0));

        public LoadingDialog()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}