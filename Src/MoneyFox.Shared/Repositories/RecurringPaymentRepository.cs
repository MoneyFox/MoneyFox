using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Repositories
{
    public class RecurringPaymentRepository : IRepository<RecurringPayment>
    {
        private readonly IDataAccess<RecurringPayment> dataAccess;
        private ObservableCollection<RecurringPayment> data;

        public RecurringPaymentRepository(IDataAccess<RecurringPayment> dataAccess)
        {
            this.dataAccess = dataAccess;

            Data = new ObservableCollection<RecurringPayment>();
            Load();
        }

        public ObservableCollection<RecurringPayment> Data
        {
            get { return data; }
            set
            {
                if (Equals(data, value))
                {
                    return;
                }
                data = value;
            }
        }

        public RecurringPayment FindById(int id) => data.FirstOrDefault(p => p.Id == id);

        public bool Delete(RecurringPayment paymentToDelete)
        {
            data.Remove(paymentToDelete);
            return dataAccess.DeleteItem(paymentToDelete);
        }

        public void Load(Expression<Func<RecurringPayment, bool>> filter = null)
        {
            Data.Clear();

            foreach (var recPayment in dataAccess.LoadList(filter))
            {
                Data.Add(recPayment);
            }
        }

        public bool Save(RecurringPayment payment)
        {
            if (payment.Id == 0)
            {
                data.Add(payment);
            }
            return dataAccess.SaveItem(payment);
        }
    }
}