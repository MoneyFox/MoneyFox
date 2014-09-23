using System;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;

namespace MoneyManager.UserControls
{
    public sealed partial class AddTransactionUserControl
    {
        public AddTransactionUserControl()
        {
            InitializeComponent();

            ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction.Date = DateTime.Now;
            

        }
    }
}