using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.FullFragging.Fragments;
using MvvmCross.Platform;

namespace MoneyFox.Droid.Dialogs {
    public class ModifyCategoryDialog : MvxDialogFragment<ModifyCategoryDialogViewModel> {
        public ModifyCategoryDialog(Category category = null) {
            ViewModel = Mvx.Resolve<ModifyCategoryDialogViewModel>();

            if (category != null) {
                ViewModel.IsEdit = true;
                ViewModel.Selected = category;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            ViewModel.LoadedCommand.Execute();

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.dialog_modify_category, container, true);

            // Handle dismiss button click
            view.FindViewById<TextView>(Resource.Id.text_view_save_category).Click += Dismiss;

            return view;
        }

        private void Dismiss(object sender, EventArgs e) {
            Dismiss();
        }

        public override void OnDismiss(IDialogInterface dialog) {
            (Context as IDialogCloseListener)?.HandleDialogClose();
            base.OnDismiss(dialog);
        }

        public override void OnResume() {
            // Auto size the dialog based on it's contents
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            base.OnResume();
        }
    }
}