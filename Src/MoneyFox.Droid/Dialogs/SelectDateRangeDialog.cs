using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MoneyFox.Droid.Activities;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Droid.Dialogs
{
    public class SelectDateRangeDialog : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        private readonly Context context;
        private readonly SelectDateRangeDialogViewModel viewModel;
        private Button callerButton;
        private Button doneButton;
        private Button selectEndDateButton;

        private Button selectStartDateButton;

        public SelectDateRangeDialog(Context context)
        {
            this.context = context;

            viewModel = Mvx.Resolve<SelectDateRangeDialogViewModel>();
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);

            if (callerButton == selectStartDateButton)
            {
                viewModel.StartDate = date;
            }
            else if (callerButton == selectEndDateButton)
            {
                viewModel.EndDate = date;
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
            doneButton = view.FindViewById<Button>(Resource.Id.button_done);
            doneButton.Click += DoneButtonClick;

            AssignDateToButtons();

            return view;
        }

        private void SelectEndDate(object sender, EventArgs e)
        {
            callerButton = sender as Button;
            var dialog = new DatePickerDialogFragment(context, DateTime.Now, this);
            dialog.Show(FragmentManager.BeginTransaction(), Strings.SelectDateTitle);
        }

        private void SelectStartDate(object sender, EventArgs e)
        {
            callerButton = sender as Button;
            var dialog = new DatePickerDialogFragment(context, DateTime.Now, this);
            dialog.Show(FragmentManager.BeginTransaction(), Strings.SelectDateTitle);
        }

        private void DoneButtonClick(object sender, EventArgs e)
        {
            viewModel.DoneCommand.Execute();
            Dismiss();
        }

        private void AssignDateToButtons()
        {
            selectStartDateButton.Hint = viewModel.StartDate.ToString("d");
            selectEndDateButton.Hint = viewModel.EndDate.ToString("d");
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            (Context as IDateRangeDialogCloseListener)?.HandleDialogClose();

            base.OnDismiss(dialog);
        }
    }
}