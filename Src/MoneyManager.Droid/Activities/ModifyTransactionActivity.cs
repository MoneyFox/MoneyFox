using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Fragments;
using MoneyManager.Localization;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "ModifyTransactionActivity")]
    public class ModifyTransactionActivity : MvxActivity, DatePickerDialog.IOnDateSetListener
    {
        /// <summary>
        ///     Used to determine which button called the date picker
        /// </summary>
        private Button callerButton;

        private Button categoryButton;
        private Button enddateButton;
        private Button transactionDateButton;

        public new ModifyTransactionViewModel ViewModel
        {
            get { return (ModifyTransactionViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);

            if (callerButton == transactionDateButton)
            {
                ViewModel.SelectedTransaction.Date = date;
            }
            else if (callerButton == enddateButton)
            {
                ViewModel.EndDate = date;
            }
        }

        /// <summary>
        ///     Raises the create event.
        /// </summary>
        /// <param name="bundle">Saved instance state.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ModifyTransactionLayout);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.Title = ViewModel.Title;

            categoryButton = FindViewById<Button>(Resource.Id.category);
            transactionDateButton = FindViewById<Button>(Resource.Id.transactiondate);
            enddateButton = FindViewById<Button>(Resource.Id.enddate);

            categoryButton.Click += SelectCategory;
            transactionDateButton.Click += ShowDatePicker;
            enddateButton.Click += ShowDatePicker;
        }

        private void SelectCategory(object sender, EventArgs e)
        {
            ViewModel.GoToSelectCategorydialogCommand.Execute();
        }

        private void ShowDatePicker(object sender, EventArgs eventArgs)
        {
            callerButton = sender as Button;
            var dialog = new DatePickerDialogFragment(this, DateTime.Now, this);
            dialog.Show(FragmentManager.BeginTransaction(), Strings.SelectDateTitle);
        }

        /// <summary>
        ///     Initialize the contents of the Activity's standard options menu.
        /// </summary>
        /// <param name="menu">The options menu in which you place your items.</param>
        /// <returns>To be added.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(ViewModel.IsEdit ? Resource.Menu.ModificationMenu : Resource.Menu.AddMenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        ///     This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <param name="item">The menu item that was selected.</param>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.action_save:
                    ViewModel.SaveCommand.Execute(null);
                    return true;

                case Resource.Id.action_delete:
                    ViewModel.DeleteCommand.Execute(null);
                    return true;

                default:
                    return false;
            }
        }
    }
}