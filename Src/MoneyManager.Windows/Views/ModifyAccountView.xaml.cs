using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using MoneyManager.Core.Helper;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class ModifyAccountView
    {
        public ModifyAccountView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<ModifyAccountViewModel>();
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

        private void ReplaceSeparatorChar(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCurrentBalance.Text)) return;

            //cursorpositon to set the position back after the formating
            var cursorposition = TextBoxCurrentBalance.SelectionStart;

            //replace either a comma with the decimal separator for the current culture to avoid parsing errors.
            TextBoxCurrentBalance.Text = TextBoxCurrentBalance.Text
                .Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

            if (string.IsNullOrEmpty(TextBoxCurrentBalance.Text)) return;
            //replace either a dot with the decimal separator for the current culture to avoid parsing errors.
            TextBoxCurrentBalance.Text = TextBoxCurrentBalance.Text
                .Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

            if (string.IsNullOrEmpty(TextBoxCurrentBalance.Text)) return;

            var formattedText =
                Utilities.FormatLargeNumbers(Convert.ToDouble(TextBoxCurrentBalance.Text, CultureInfo.CurrentCulture));

            cursorposition = AdjustCursorPosition(formattedText, cursorposition);

            TextBoxCurrentBalance.Text = formattedText;

            //set the cursor back to the last positon to avoid jumping around
            TextBoxCurrentBalance.Select(cursorposition, 0);
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