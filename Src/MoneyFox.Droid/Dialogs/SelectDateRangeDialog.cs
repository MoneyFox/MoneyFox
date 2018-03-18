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
    public class SelectDateRangeDialog : MvxDialogFragment<SelectDateRangeDialogViewModel>,
        DatePickerDialog.IOnDateSetListener
    {
        private Button callerButton;
        private TextView doneTextView;
        private Button selectEndDateButton;
        private Button selectStartDateButton;

        public SelectDateRangeDialog()
        {
            ViewModel = Mvx.Resolve<SelectDateRangeDialogViewModel>();
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);

            if (callerButton == selectStartDateButton)
            {
                ViewModel.StartDate = date;
            }
            else if (callerButton == selectEndDateButton)
            {
                ViewModel.EndDate = date;
            }
            AssignDateToButtons();
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = new Dialog(Activity);
            dialog.SetContentView(Resource.Layout.dialog_select_date_range);

            // Handle select start date button click
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
            ViewModel.DoneCommand.Execute();
            Dismiss();
        }

        private void AssignDateToButtons()
        {
            selectStartDateButton.Hint = ViewModel.StartDate.ToString("d");
            selectEndDateButton.Hint = ViewModel.EndDate.ToString("d");
        }
    }
}