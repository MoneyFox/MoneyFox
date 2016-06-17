using System;
using Android.App;
using Android.Content;
using Android.OS;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Droid.Dialogs
{
    /// <summary>
    ///     Provides an Dialog to select a start and an end date.
    /// </summary>
    public class DatePickerDialogFragment : DialogFragment
    {
        private readonly Context context;
        private readonly DateTime date;
        private readonly DatePickerDialog.IOnDateSetListener listener;

        public DatePickerDialogFragment(Context context, DateTime date, DatePickerDialog.IOnDateSetListener listener)
        {
            this.context = context;
            this.date = date;
            this.listener = listener;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
            => new DatePickerDialog(context, listener, date.Year, date.Month - 1, date.Day);

        public override void OnDismiss(IDialogInterface dialog) {
            (Context as IDialogCloseListener)?.HandleDialogClose();

            base.OnDismiss(dialog);
        }
    }
}