using System;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class CategoryListView
    {
        public override bool ShowHeader => false;

        public CategoryListView()
        {
            InitializeComponent();
        }

        private async void AddNewCategoryClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new AddCategoryDialog().ShowAsync();
        }
    }
}
