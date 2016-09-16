using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using System.Collections.ObjectModel;
using MoneyFox.Shared.Repositories;


namespace MoneyFox.Shared.Manager
{
    public class EndOfMonthManager : IEndOfMonthManager
    {
        private readonly IPaymentRepository paymentRepository;
        
        public EndOfMonthManager( IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
            
        }
        
        public string DetermineEnd(int accountID, double startBalance)
        {

            double myTemp = startBalance;
            DateTime myTime = DateTime.Now;
            foreach (Payment x in paymentRepository.GetList())
            {
                if(x.TargetAccountId == accountID)
                {
                    myTemp += x.Amount;
                }
                if (x.ChargedAccountId == accountID)
                {
                    myTemp -= x.Amount;
                }
            }
            if (myTemp < 0)
            {
                return "BAD";
            }
            return "GOOD";

        }
        

        public string Hope()
        {
            return "HOPE";
        }
    }
}