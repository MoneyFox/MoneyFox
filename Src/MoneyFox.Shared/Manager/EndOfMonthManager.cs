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

        public EndOfMonthManager(IPaymentRepository paymentRepository, IAccountRepository accountRepository)
        {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;

        }


        public void AssignToAccounts()
        {
            foreach (Account x in accountRepository.GetList())
            {
                DeterminEndThroughAccounts(x);
            }
        }

        public void DeterminEndThroughAccounts(Account argAccount)
        {
            double tempBalance = argAccount.CurrentBalance;
            DateTime tempTime = DateTime.Now;

            foreach (Payment x in paymentRepository.GetList())
            {
                if (x.TargetAccountId == argAccount.Id)
                {
                    tempBalance += x.Amount;
                }
                else if (x.ChargedAccountId == argAccount.Id && x.TargetAccount == null)
                {
                    tempBalance -= x.Amount;
                }
                else if (x.ChargedAccountId == argAccount.Id && x.TargetAccount != null)
                {
                    tempBalance -= x.Amount;
                }
            }
            if (tempBalance < 0)
            {
                argAccount.EndMonthWarning = "Negative at end of month";
            }
            else
            {
                argAccount.EndMonthWarning = " ";
            }
        }

    }
}
