using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using System.Collections.ObjectModel;
using MoneyFox.Shared.DataAccess;
using MoneyFox.Shared.Repositories;
using System;
using System.Linq;
using SQLite.Net;
using System.Linq;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MvvmCross.Plugins.File;
using MvvmCross.Plugins.Sqlite;
using SQLite.Net;
using SQLite.Net.Async;



namespace MoneyFox.Shared.Manager
{
    public class EndOfMonthManager
    {

        public double totals;

        public EndOfMonthManager(double x, int id)
        {
            totals = x;
            Id = id;
        }
         int Id;
        public double decide()
        {
            return totals;
        }

        private static DatabaseManager myManager;

        public static IDataAccess<Payment> dataAccess = new PaymentDataAccess(myManager);

        static PaymentRepository paymentRepository = new PaymentRepository(dataAccess);
        public ObservableCollection<Payment> RelatedPayments { get; set; }
        

        public string returnValue()
        {
            RelatedPayments = new ObservableCollection<Payment>(paymentRepository
                .GetList(x => x.ChargedAccountId == Id || x.TargetAccountId == Id));

            foreach (Payment x in RelatedPayments)
            {
                if (x.Date.Month == DateTime.Today.Month)
                {
                    if (x.TargetAccountId == Id)
                    {
                        totals -= x.Amount;
                    }
                    else
                    {
                        totals += x.Amount;
                    }
                }
            }
            if (totals < 0)
            {
                return "NO GOOD";
            }
                return "GOOD";
        }
    }
}