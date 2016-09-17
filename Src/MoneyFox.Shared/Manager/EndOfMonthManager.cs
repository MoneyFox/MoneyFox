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
        private readonly IAccountRepository accountRepository;
        
        public EndOfMonthManager( IPaymentRepository paymentRepository, IAccountRepository accountRepository)
        {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
            
        }
       

        public void AssignToAccounts()
        {
            foreach(Account x in accountRepository.GetList())
            {
                DeterminEndThroughAccounts(x);
            }
        }


        /*
        DETERMINES STRING VALUE PER ACCOUNT 
        */
        public void DeterminEndThroughAccounts(Account argAccount)
        {
            double tempBalance = argAccount.CurrentBalance;
            DateTime tempTime = DateTime.Now;

            foreach (Payment x in paymentRepository.GetList())
            {
                if(x.TargetAccountId== argAccount.Id && x.Date.Month == tempTime.Date.Month)
                {
                    tempBalance += x.Amount;
                }
                else if(x.ChargedAccountId == argAccount.Id && x.TargetAccount==null && x.Date.Month == tempTime.Date.Month)
                {
                    tempBalance += x.Amount;
                }   
                else if(x.ChargedAccountId == argAccount.Id && x.TargetAccount!=null && x.Date.Month == tempTime.Date.Month)
                {
                    tempBalance -= x.Amount;
                }             
            }
            if (tempBalance < 0)
            {
                argAccount.EndMonthWarning = "ACCOUNT WILL BE NEGATIVE AT END OF MONTH";
            }
            else
            {
                argAccount.EndMonthWarning = " ";
            }
        }
        
        
         
      /*  public string DetermineEnd(int accountID, double startBalance)
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

        }*/
    }
}