namespace MoneyFox.Ui.Views.Statistics;

using CommunityToolkit.Mvvm.ComponentModel;

public class CategoryOverviewViewModel : ObservableObject
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
        set => SetProperty(field: ref categoryId, newValue: value);
    }

    /// <summary>
    ///     Value of this item
    /// </summary>
    public decimal Value
    {
        get => value;
        set => SetProperty(field: ref this.value, newValue: value);
    }

    /// <summary>
    ///     Average of this item
    /// </summary>
    public decimal Average
    {
        get => average;
        set => SetProperty(field: ref average, newValue: value);
    }

    /// <summary>
    ///     Value of this item
    /// </summary>
    public decimal Percentage
    {
        get => percentage;
        set => SetProperty(field: ref percentage, newValue: value);
    }

    /// <summary>
    ///     Label to show in the chart
    /// </summary>
    public string Label
    {
        get => label;
        set => SetProperty(field: ref label, newValue: value);
    }
}
