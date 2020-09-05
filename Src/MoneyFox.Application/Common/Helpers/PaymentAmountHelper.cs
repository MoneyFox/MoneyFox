using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace MoneyFox.Application.Common.Helpers
{
    public static class PaymentAmountHelper
    {
        public static decimal AddPaymentToBalance(Payment payment, List<Account> excluded, decimal currentBalance)
        {
            switch(payment.Type)
            {
                case PaymentType.Expense:
                    currentBalance -= payment.Amount;
                    break;

                case PaymentType.Income:
                    currentBalance += payment.Amount;
                    break;

                case PaymentType.Transfer:
                    currentBalance = CalculateBalanceForTransfer(excluded, currentBalance, payment);
                    break;

                default:
                    throw new InvalidPaymentTypeException();
            }

            return currentBalance;
        }

        private static decimal CalculateBalanceForTransfer(List<Account> excluded, decimal balance, Payment payment)
        {
            foreach(Account account in excluded)
            {
                if(Equals(account.Id, payment.ChargedAccount!.Id))
                {
                    //Transfer from excluded account
                    balance += payment.Amount;
                }

                if(payment.TargetAccount == null)
                {
                    throw new InvalidOperationException($"Navigation Property not initialized properly: {nameof(payment.TargetAccount)}");
                }

                if(Equals(account.Id, payment.TargetAccount.Id))
                {
                    //Transfer to excluded account
                    balance -= payment.Amount;
                }
            }

            return balance;
        }
    }
}
