using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class CategoryOverviewViewModel : BaseViewModel
    {
        private string label;
        private double value;
        private double average;
        private double percentage;

        /// <summary>
        ///     Value of this item
        /// </summary>
        public double Value
        {
            get => value;
            set
            {
                if (Math.Abs(this.value - value) < 0.01) return;
                this.value = value;
                RaisePropertyChanged();
            }
        }   
        
        /// <summary>
        ///     Average of this item
        /// </summary>
        public double Average
        {
            get => average;
            set
            {
                if (Math.Abs(this.average - value) < 0.01) return;
                this.average = value;
                RaisePropertyChanged();
            }

        }

        /// <summary>
        ///     Value of this item
        /// </summary>
        public double Percentage
        {
            get => percentage;
            set
            {
                if (Math.Abs(this.value - value) < 0.01) return;
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
                if (label == value) return;
                label = value;
                RaisePropertyChanged();
            }
        }
    }
}