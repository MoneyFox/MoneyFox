using System;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Manager
{
    public class EndOfMonthManager : IEndOfMonthManager
    {
        private readonly IAccountRepository accountRepository;
        private readonly IPaymentRepository paymentRepository;

        public EndOfMonthManager(IPaymentRepository paymentRepository, IAccountRepository accountRepository)
        {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
        }


        public void AssignToAccounts()
        {
            foreach (var x in accountRepository.GetList())
            {
                DeterminEndThroughAccounts(x);
            }
        }

        public void DeterminEndThroughAccounts(Account argAccount)
        {
            var tempBalance = argAccount.CurrentBalance;
            var tempTime = DateTime.Now;

            foreach (var x in paymentRepository.GetList())
            {
                if (x.TargetAccountId == argAccount.Id)
                {
                    tempBalance += x.Amount;
                }
                else if ((x.ChargedAccountId == argAccount.Id) && (x.TargetAccount == null) &&
                         (tempTime.Month == x.Date.Month))
                {
                    tempBalance -= x.Amount;
                }
                else if ((x.ChargedAccountId == argAccount.Id) && (x.TargetAccount != null) &&
                         (tempTime.Month == x.Date.Month))
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