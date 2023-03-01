namespace MoneyFox.Ui.Views.Statistics;

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
        set => SetProperty(property: ref categoryId, value: value);
    }

    /// <summary>
    ///     Value of this item
    /// </summary>
    public decimal Value
    {
        get => value;
        set => SetProperty(property: ref this.value, value: value);
    }

    /// <summary>
    ///     Average of this item
    /// </summary>
    public decimal Average
    {
        get => average;
        set => SetProperty(property: ref average, value: value);
    }

    /// <summary>
    ///     Value of this item
    /// </summary>
    public decimal Percentage
    {
        get => percentage;
        set => SetProperty(property: ref percentage, value: value);
    }

    /// <summary>
    ///     Label to show in the chart
    /// </summary>
    public string Label
    {
        get => label;
        set => SetProperty(property: ref label, value: value);
    }
}
