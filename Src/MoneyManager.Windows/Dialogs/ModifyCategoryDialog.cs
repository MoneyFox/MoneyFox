using Windows.System;
using Windows.UI.Xaml.Input;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;
using MvvmCross.Platform;

namespace MoneyManager.Windows.Dialogs
{
    public sealed partial class ModifyCategoryDialog
    {
        public ModifyCategoryDialog(Category category = null)
        {
            InitializeComponent();

            DataContext = Mvx.Resolve<CategoryDialogViewModel>();

            if (category != null)
            {
                ((CategoryDialogViewModel) DataContext).IsEdit = true;
                ((CategoryDialogViewModel) DataContext).Selected = category;
            }
        }

        private void TextBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ((CategoryDialogViewModel) DataContext).DoneCommand.Execute();
                Hide();
            }
        }
    }
}