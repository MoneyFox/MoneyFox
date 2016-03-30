using Windows.System;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.Model;
using MoneyFox.Core.ViewModels;
using MoneyFox.Foundation.Model;

namespace MoneyFox.Windows.Views.Dialogs
{
    public sealed partial class ModifyCategoryDialog
    {
        public ModifyCategoryDialog(Category category = null)
        {
            InitializeComponent();

            DataContext = ServiceLocator.Current.GetInstance<CategoryDialogViewModel>();

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
                ((CategoryDialogViewModel) DataContext).DoneCommand.Execute(null);
                Hide();
            }
        }
    }
}