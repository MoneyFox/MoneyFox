using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using MoneyManager.Core.ViewModels;
using MoneyManager.Core.ViewModels.Dialogs;

namespace MoneyManager.Droid.Fragments
{
    public class ModifyCategoryDialog : MvxDialogFragment
    {
        public new CategoryDialogViewModel ViewModel
        {
            get { return (CategoryDialogViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewModel.LoadedCommand.Execute();

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.ModifyCategoryDialogLayout, container, true);

            // Handle dismiss button click
            var button  = view.FindViewById<Button>(Resource.Id.button_save_category);
            button.Click += Dismiss;

            return view;
        }

        private void Dismiss(object sender, EventArgs e)
        {
            Dismiss();
        }

        public override void OnResume()
        {
            // Auto size the dialog based on it's contents
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            // Make sure there is no background behind our view
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            // Disable standard dialog styling/frame/theme: our custom view should create full UI
            SetStyle(StyleNormal, Android.Resource.Style.Theme);

            base.OnResume();
        }
    }
}