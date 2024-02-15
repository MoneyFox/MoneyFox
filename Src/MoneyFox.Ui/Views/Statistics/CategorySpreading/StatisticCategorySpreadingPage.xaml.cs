namespace MoneyFox.Ui.Views.Statistics.CategorySpreading;

using Common.Navigation;

public partial class StatisticCategorySpreadingPage : IBindablePage
{
    public StatisticCategorySpreadingPage()
    {
        InitializeComponent();
    }

    private void NumberOfCategoriesEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!int.TryParse(e.NewTextValue, out var numberOfCategories))
        {
            // clean out invalid characters
            numberOfCategoriesEntry.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
        }
    }
}
