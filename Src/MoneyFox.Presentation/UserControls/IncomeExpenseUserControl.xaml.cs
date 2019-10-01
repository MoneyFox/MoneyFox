using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncomeExpenseUserControl : ContentView
    {
        public IncomeExpenseUserControl()
        {
            InitializeComponent();
        }
    }
}