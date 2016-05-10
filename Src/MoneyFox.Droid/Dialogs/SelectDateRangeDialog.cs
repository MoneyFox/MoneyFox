using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using Android.OS;
using Android.Support.V4.App;

namespace MoneyFox.Droid.Dialogs
{
    public class SelectDateRangeDialog : DialogFragment
    {
        private Context context;

        private Button doneButton;

        public SelectDateRangeDialog(Context context)
        {
            this.context = context;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Android 3.x+ still wants to show title: disable
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            // Create our view
            var view = inflater.Inflate(Resource.Layout.dialog_select_date_range, container, true);

            // Handle dismiss button click
            doneButton = view.FindViewById<Button>(Resource.Id.button_done);
            doneButton.Click += DoneButtonClick;

            return view;
        }

        private void DoneButtonClick(object sender, EventArgs e)
        {
            Dismiss();
        }
    }
}