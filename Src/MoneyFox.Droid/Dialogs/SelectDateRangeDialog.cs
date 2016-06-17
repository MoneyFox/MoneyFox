using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Droid.FullFragging.Fragments;
using MoneyFox.Droid.Fragments;

namespace MoneyFox.Droid.Dialogs
{
    public class SelectDateRangeDialog : MvxDialogFragment<SelectDateRangeDialogViewModel>, DatePickerDialog.IOnDateSetListener
    {
        private Button callerButton;
        private Button selectEndDateButton;
        private Button selectStartDateButton;
        private TextView doneTextView;

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Android 3.x+ still wants to show title: disable
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            // Create our view
            var view = inflater.Inflate(Resource.Layout.dialog_select_date_range, container, true);

            // Handle select start date button click
            selectStartDateButton = view.FindViewById<Button>(Resource.Id.button_start_date);
            selectStartDateButton.Click += SelectStartDate;

            // Handle select end date button click
            selectEndDateButton = view.FindViewById<Button>(Resource.Id.button_end_date);
            selectEndDateButton.Click += SelectEndDate;

            // Handle dismiss button click
            doneTextView = view.FindViewById<TextView>(Resource.Id.textview_done);
            doneTextView.Click += DoneButtonClick;

            AssignDateToButtons();

            return view;
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

        public override void OnDismiss(IDialogInterface dialog)
        {
            (Context as IDialogCloseListener)?.HandleDialogClose();

            base.OnDismiss(dialog);
        }
    }
}