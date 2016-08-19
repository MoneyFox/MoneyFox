using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Repositories
{
    public class RecurringPaymentRepository : IRecurringPaymentRepository
    {
        private readonly IDataAccess<RecurringPayment> dataAccess;

        private List<RecurringPayment> data;

        public RecurringPaymentRepository(IDataAccess<RecurringPayment> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<RecurringPayment> GetList(Expression<Func<RecurringPayment, bool>> filter = null)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            return filter != null ? data.Where(filter.Compile()) : data;
        }

        public RecurringPayment FindById(int id)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }
            return data.FirstOrDefault(p => p.Id == id);
        }


        public bool Delete(RecurringPayment paymentToDelete)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            data.Remove(paymentToDelete);
            return dataAccess.DeleteItem(paymentToDelete);
        }

        public void Load(Expression<Func<RecurringPayment, bool>> filter = null)
        {
            data = dataAccess.LoadList();
        }

        public bool Save(RecurringPayment payment)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            if (payment.Id == 0)
            {
                data.Add(payment);
            }
            return dataAccess.SaveItem(payment);
        }
    }
}