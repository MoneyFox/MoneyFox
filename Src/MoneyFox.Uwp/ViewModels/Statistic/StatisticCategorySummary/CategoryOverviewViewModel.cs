using GalaSoft.MvvmLight;
using MoneyFox.Uwp.Groups;
using MoneyFox.Uwp.ViewModels.Payments;
using System;
using System.Collections.ObjectModel;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary
{
    public class CategoryOverviewViewModel : ViewModelBase
    {
        private const decimal DECIMAL_DELTA = 0.01m;
        private decimal average;
        private int categoryId;
        private string label = "";
        private decimal percentage;

        private ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> source
            = new ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>>();

        private decimal value;

        /// <summary>
        ///     Value of this item
        /// </summary>
        public int CategoryId
        {
            get => categoryId;
            set
            {
                if(categoryId == value)
                {
                    return;
                }

                categoryId = value;
                RaisePropertyChanged();
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
                if(Math.Abs(this.value - value) < DECIMAL_DELTA)
                {
                    return;
                }

                this.value = value;
                RaisePropertyChanged();
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
                if(Math.Abs(average - value) < DECIMAL_DELTA)
                {
                    return;
                }

                average = value;
                RaisePropertyChanged();
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
                if(Math.Abs(this.value - value) < DECIMAL_DELTA)
                {
                    return;
                }

                percentage = value;
                RaisePropertyChanged();
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
                if(label == value)
                {
                    return;
                }

                label = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Source for the payment list
        /// </summary>
        public ObservableCollection<DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>> Source
        {
            get => source;
            private set
            {
                if(source == value)
                {
                    return;
                }

                source = value;
                RaisePropertyChanged();
            }
        }
    }
}