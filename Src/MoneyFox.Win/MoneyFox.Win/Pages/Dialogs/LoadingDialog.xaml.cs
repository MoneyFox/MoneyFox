using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MoneyFox.Win.Pages.Dialogs
{
    public sealed partial class LoadingDialog : ContentDialog
    {
        public LoadingDialog()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LoadingDialog), new PropertyMetadata(0));
    }
}