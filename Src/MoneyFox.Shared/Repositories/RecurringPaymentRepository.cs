using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;

namespace MoneyFox.Shared.Repositories
{
    public class RecurringPaymentRepository : IRecurringPaymentRepository
    {
        private readonly IDataAccess<RecurringPaymentViewModel> dataAccess;

        private List<RecurringPaymentViewModel> data;

        public RecurringPaymentRepository(IDataAccess<RecurringPaymentViewModel> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<RecurringPaymentViewModel> GetList(Expression<Func<RecurringPaymentViewModel, bool>> filter = null)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            return filter != null ? data.Where(filter.Compile()) : data;
        }

        public RecurringPaymentViewModel FindById(int id)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }
            return data.FirstOrDefault(p => p.Id == id);
        }


        public bool Delete(RecurringPaymentViewModel paymentToDelete)
        {
            if (data == null)
            {
                data = dataAccess.LoadList();
            }

            data.Remove(paymentToDelete);
            return dataAccess.DeleteItem(paymentToDelete);
        }

        public void Load(Expression<Func<RecurringPaymentViewModel, bool>> filter = null)
        {
            data = dataAccess.LoadList();
        }

        public bool Save(RecurringPaymentViewModel payment)
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