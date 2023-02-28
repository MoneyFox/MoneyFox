namespace MoneyFox.Ui.Views.Statistics;

using CommunityToolkit.Mvvm.ComponentModel;

public class CategoryOverviewViewModel : ObservableViewModelBase
{
    private decimal average;

    private int? categoryId;
    private string label = "";
    private decimal percentage;
    private decimal value;

    /// <summary>
    ///     Value of this item
    /// </summary>
    public int? CategoryId
    {
        get => categoryId;
        set => SetProperty(ref categoryId, value);
    }

    /// <summary>
    ///     Value of this item
    /// </summary>
    public decimal Value
    {
        get => value;
        set => SetProperty(ref this.value, value);
    }

    /// <summary>
    ///     Average of this item
    /// </summary>
    public decimal Average
    {
        get => average;
        set => SetProperty(ref average, value);
    }

    /// <summary>
    ///     Value of this item
    /// </summary>
    public decimal Percentage
    {
        get => percentage;
        set => SetProperty(ref percentage, value);
    }

    /// <summary>
    ///     Label to show in the chart
    /// </summary>
    public string Label
    {
        get => label;
        set => SetProperty(ref label, value);
    }
}
