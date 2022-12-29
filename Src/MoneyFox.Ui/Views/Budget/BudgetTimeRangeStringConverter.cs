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
            BudgetTimeRange.YearToDate => Translations.YearToDateLabel,
            BudgetTimeRange.Last1Year => Translations.LastYearLabel,
            BudgetTimeRange.Last2Years => Translations.LastTwoYearsLabel,
            BudgetTimeRange.Last3Years => Translations.LastThreeYearsLabel,
            BudgetTimeRange.Last5Years => Translations.LastFiveYearsLabel
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
