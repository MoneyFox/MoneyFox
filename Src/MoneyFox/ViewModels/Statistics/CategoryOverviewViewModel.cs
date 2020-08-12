using GalaSoft.MvvmLight;
using System;

namespace MoneyFox.ViewModels.Statistics
{
    public class CategoryOverviewViewModel : ViewModelBase
    {
        private int categoryId;
        private string label;
        private decimal value;
        private decimal average;
        private decimal percentage;

        /// <summary>
        /// Value of this item
        /// </summary>
        public int CategoryId
        {
            get => categoryId;
            set
            {
                if(categoryId == value)
                    return;
                categoryId = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Value of this item
        /// </summary>
        public decimal Value
        {
            get => value;
            set
            {
                if(Math.Abs(this.value - value) < 0.01m)
                    return;
                this.value = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Average of this item
        /// </summary>
        public decimal Average
        {
            get => average;
            set
            {
                if(Math.Abs(average - value) < 0.01m)
                    return;
                average = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Value of this item
        /// </summary>
        public decimal Percentage
        {
            get => percentage;
            set
            {
                if(Math.Abs(this.value - value) < 0.01m)
                    return;
                percentage = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Label to show in the chart
        /// </summary>
        public string Label
        {
            get => label;
            set
            {
                if(label == value)
                    return;
                label = value;
                RaisePropertyChanged();
            }
        }
    }
}
