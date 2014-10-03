using System;
using Windows.UI.Xaml;

namespace MoneyManager.UserControls
{
    public sealed partial class AddAccountUserControl
    {
        public AddAccountUserControl()
        {
            InitializeComponent();
        }

        private void RemoveZeroOnFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxCurrentBalance.Text == "0")
            {
                TextBoxCurrentBalance.Text = String.Empty;
            }

            TextBoxCurrentBalance.SelectAll();
        }

        private void AddZeroIfEmpty(object sender, RoutedEventArgs e)
        {
            if (TextBoxCurrentBalance.Text == String.Empty)
            {
                TextBoxCurrentBalance.Text = "0";
            }
        }
    }
}