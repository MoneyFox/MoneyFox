using MoneyManager;
using MoneyTracker.Src;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace MoneyTracker.UserControls
{
    public sealed partial class AddTransactionUserControl
    {
        public AddTransactionUserControl()
        {
            InitializeComponent();

            var recurrences = new List<RecurrenceType>()
            {
                RecurrenceType.Weekly, RecurrenceType.Monthly
            };
            ComboBoxRecurrenceType.ItemsSource = recurrences;
        }

        public string GetRecurrenceType()
        {
            return ComboBoxRecurrenceType.SelectedValue.ToString();
        }

        public DateTime GetEndDate()
        {
            return DatePickerEndDate.Date.Date;
        }
    }
}