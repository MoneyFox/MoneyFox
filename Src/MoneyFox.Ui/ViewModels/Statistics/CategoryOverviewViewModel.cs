namespace MoneyFox.Ui.ViewModels.Statistics;

using CommunityToolkit.Mvvm.ComponentModel;

public class CategoryOverviewViewModel : ObservableObject
{
    private const decimal DECIMAL_DELTA = 0.01m;
    private decimal average;

    private int categoryId;
    private string label = "";
    private decimal percentage;
    private decimal value;

    /// <summary>
    ///     Value of this item
    /// </summary>
    public int CategoryId
    {
        get => categoryId;

        set
        {
            if (categoryId == value)
            {
                return;
            }

            categoryId = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Value of this item
    /// </summary>
    public decimal Value
    {
        get => value;

        set
        {
            if (Math.Abs(this.value - value) < DECIMAL_DELTA)
            {
                return;
            }

            this.value = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Average of this item
    /// </summary>
    public decimal Average
    {
        get => average;

        set
        {
            if (Math.Abs(average - value) < DECIMAL_DELTA)
            {
                return;
            }

            average = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Value of this item
    /// </summary>
    public decimal Percentage
    {
        get => percentage;

        set
        {
            if (Math.Abs(this.value - value) < DECIMAL_DELTA)
            {
                return;
            }

            percentage = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Label to show in the chart
    /// </summary>
    public string Label
    {
        get => label;

        set
        {
            if (label == value)
            {
                return;
            }

            label = value;
            OnPropertyChanged();
        }
    }
}
