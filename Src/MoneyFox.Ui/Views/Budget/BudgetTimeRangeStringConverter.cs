namespace MoneyFox.Ui.Views.Budget;

using System.Globalization;
using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Core.Resources;

internal class BudgetTimeRangeStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var timeRange = (BudgetTimeRange)value;

        return timeRange switch
        {
            BudgetTimeRange.YearToDate => Strings.YearToDateLabel,
            BudgetTimeRange.Last1Year => Strings.LastYearLabel,
            BudgetTimeRange.Last2Years => Strings.LastTwoYearsLabel,
            BudgetTimeRange.Last3Years => Strings.LastThreeYearsLabel,
            BudgetTimeRange.Last5Years => Strings.LastFiveYearsLabel
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
