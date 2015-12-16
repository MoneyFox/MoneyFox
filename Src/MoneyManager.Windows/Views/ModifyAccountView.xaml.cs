using System;
using System.Globalization;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using MoneyManager.Core.Helpers;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Exceptions;
using MoneyManager.Localization;

namespace MoneyManager.Windows.Views
{
    public sealed partial class ModifyAccountView
    {
        public ModifyAccountView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<ModifyAccountViewModel>();

            // code to handle bottom app bar when keyboard appears
            // workaround since otherwise the keyboard would overlay some controls
            InputPane.GetForCurrentView().Showing += (s, args) => { BottomCommandBar.Visibility = Visibility.Collapsed; };
            InputPane.GetForCurrentView().Hiding += (s, args2) =>
            {
                if (BottomCommandBar.Visibility == Visibility.Collapsed)
                {
                    BottomCommandBar.Visibility = Visibility.Visible;
                }
            };
        }

        private void RemoveZeroOnFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxCurrentBalance.Text == "0")
            {
                TextBoxCurrentBalance.Text = string.Empty;
            }

            TextBoxCurrentBalance.SelectAll();
        }

        private void AddZeroIfEmpty(object sender, RoutedEventArgs e)
        {
            if (TextBoxCurrentBalance.Text == string.Empty)
            {
                TextBoxCurrentBalance.Text = "0";
            }
        }

        private async void ReplaceSeparatorChar(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCurrentBalance.Text)) return;

            double amount;
            if (double.TryParse(TextBoxCurrentBalance.Text, out amount))
            {
                // todo: this try should be removeable, will see after the next version.
                try
                {
                    //cursorpositon to set the position back after the formating
                    var cursorposition = TextBoxCurrentBalance.SelectionStart;

                    var formattedText =
                        Utilities.FormatLargeNumbers(amount);

                    cursorposition = AdjustCursorPosition(formattedText, cursorposition);

                    TextBoxCurrentBalance.Text = formattedText;

                    //set the cursor back to the last positon to avoid jumping around
                    TextBoxCurrentBalance.Select(cursorposition, 0);
                }
                catch (FormatException ex)
                {
                    InsightHelper.Report(new ExtendedFormatException(ex, TextBoxCurrentBalance.Text));
                }
            }
            else if (string.Equals(TextBoxCurrentBalance.Text, Strings.HelloWorldText,
                StringComparison.CurrentCultureIgnoreCase)
                     ||
                     string.Equals(TextBoxCurrentBalance.Text, Strings.HalloWeltText,
                         StringComparison.CurrentCultureIgnoreCase))
            {
                await new MessageDialog(Strings.HelloWorldResponse).ShowAsync();
            }
        }

        /// <summary>
        ///     When the text is formated there may be more chars and the cursors positon isn't the same as befor.
        ///     That will cause a jumping cursor and uncontrolled order of input. Therefore we need to adjust the
        ///     cursor position after formating.
        /// </summary>
        /// <param name="formattedText">Text after formatting.</param>
        /// <param name="cursorposition">Position of the cursor before formatting.</param>
        /// <returns>New Cursor position.</returns>
        private int AdjustCursorPosition(string formattedText, int cursorposition)
        {
            var oldIndex =
                TextBoxCurrentBalance.Text.IndexOf(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator,
                    StringComparison.Ordinal);
            var newIndex = formattedText.IndexOf(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator,
                StringComparison.Ordinal);

            if (oldIndex != -1 && oldIndex < newIndex)
            {
                cursorposition += newIndex - oldIndex;
            }
            return cursorposition;
        }
    }
}