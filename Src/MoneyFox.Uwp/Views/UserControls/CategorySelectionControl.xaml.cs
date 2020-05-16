using System;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views.UserControls
{
    public sealed partial class CategorySelectionControl : UserControl
    {
        public CategorySelectionControl()
        {
            InitializeComponent();
        }

        private async void SelectCategory_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new SelectCategoryListView().ShowAsync();
        }
    }
}
