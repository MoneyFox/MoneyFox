using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using MoneyManager.Core.Helper;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Exceptions;

namespace MoneyManager.Windows.Views
{
    public sealed partial class ModifyTransactionView
    {
        public ModifyTransactionView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<ModifyTransactionViewModel>();
        }

        private void RemoveZeroOnFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxAmount.Text == "0")
            {
                TextBoxAmount.Text = string.Empty;
            }

            TextBoxAmount.SelectAll();
        }

        private void AddZeroIfEmpty(object sender, RoutedEventArgs e)
        {
            if (TextBoxAmount.Text == string.Empty)
            {
                TextBoxAmount.Text = "0";
            }
        }

        private void ReplaceSeparatorChar(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxAmount.Text)) return;

                //cursorpositon to set the position back after the formating
                var cursorposition = TextBoxAmount.SelectionStart;

                var formattedText =
                    Utilities.FormatLargeNumbers(Convert.ToDouble(TextBoxAmount.Text, CultureInfo.CurrentCulture));

                cursorposition = AdjustCursorPosition(formattedText, cursorposition);

                TextBoxAmount.Text = formattedText;

                //set the cursor back to the last positon to avoid jumping around
                TextBoxAmount.Select(cursorposition, 0);
            }
            catch (FormatException ex)
            {
                InsightHelper.Report(new ExtendedFormatException(ex));
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
            var oldIndex = TextBoxAmount.Text.IndexOf(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator,
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