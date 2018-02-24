using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MoneyFox.Business.ViewModels;
using MoneyFox.Droid.Fragments;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;

namespace MoneyFox.Droid.Dialogs
{
    public class SelectFilterDialog : MvxDialogFragment<SelectFilterDialogViewModel>,
        DatePickerDialog.IOnDateSetListener
    {
        private Button callerButton;
        private CheckBox checkBoxIsCleared;
        private CheckBox checkBoxIsRecurring;
        private Button selectEndDateButton;
        private Button selectStartDateButton;
        private TextView doneTextView;

        public SelectFilterDialog()
        {
            ViewModel = Mvx.Resolve<SelectFilterDialogViewModel>();
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
            
            if (callerButton == selectStartDateButton)
            {
                ViewModel.TimeRangeStart = date;
            }
            else if (callerButton == selectEndDateButton)
            {
                ViewModel.TimeRangeEnd = date;
            }
            AssignDateToButtons();
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = new Dialog(Activity);
            dialog.SetContentView(Resource.Layout.dialog_select_filter);

            // Handle select start date button click
            checkBoxIsCleared = dialog.FindViewById<CheckBox>(Resource.Id.checkbox_is_cleared);
            checkBoxIsCleared.Click += (sender, args) => ViewModel.IsClearedFilterActive = !ViewModel.IsClearedFilterActive; 

            // Handle select start date button click
            checkBoxIsRecurring = dialog.FindViewById<CheckBox>(Resource.Id.checkbox_is_recurring);
            checkBoxIsRecurring.Click += (sender, args) => ViewModel.IsRecurringFilterActive = !ViewModel.IsRecurringFilterActive;

            // Handle select end date button click
            selectStartDateButton = dialog.FindViewById<Button>(Resource.Id.button_start_date);
            selectStartDateButton.Click += SelectStartDate;

            // Handle select end date button click
            selectEndDateButton = dialog.FindViewById<Button>(Resource.Id.button_end_date);
            selectEndDateButton.Click += SelectEndDate;

            // Handle done button click
            doneTextView = dialog.FindViewById<TextView>(Resource.Id.textview_done);
            doneTextView.Click += DoneButtonClick;

            AssignDateToButtons();

            return dialog;
        }

        private void SelectEndDate(object sender, EventArgs e)
        {
            callerButton = sender as Button;
            var dialog = new DatePickerDialogFragment(Context, DateTime.Now, this);
            dialog.Show(FragmentManager.BeginTransaction(), Strings.SelectDateTitle);
        }

        private void SelectStartDate(object sender, EventArgs e)
        {
            callerButton = sender as Button;
            var dialog = new DatePickerDialogFragment(Context, DateTime.Now, this);
            dialog.Show(FragmentManager.BeginTransaction(), Strings.SelectDateTitle);
        }

        private void DoneButtonClick(object sender, EventArgs e)
        {
            Dismiss();
        }

        private void AssignDateToButtons()
        {
            selectStartDateButton.Hint = ViewModel.TimeRangeStart.ToString("d");
            selectEndDateButton.Hint = ViewModel.TimeRangeEnd.ToString("d");
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            (Context as IDialogCloseListener)?.HandleDialogClose();

            base.OnDismiss(dialog);
        }
    }
}