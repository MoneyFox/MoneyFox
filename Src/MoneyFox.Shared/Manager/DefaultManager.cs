using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;

namespace MoneyFox.Shared.Manager
{
    //TODO: Refactor to helper class
    public class DefaultManager : IDefaultManager
    {
        private readonly IUnitOfWork unitOfWork;

        public DefaultManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Account GetDefaultAccount()
        {
            if (unitOfWork.AccountRepository.Data == null)
            {
                unitOfWork.AccountRepository.Data = new ObservableCollection<Account>();
            }

            if (unitOfWork.AccountRepository.Data.Any() && SettingsHelper.DefaultAccount != -1)
            {
                return unitOfWork.AccountRepository.Data.FirstOrDefault(x => x.Id == SettingsHelper.DefaultAccount);
            }

            return unitOfWork.AccountRepository.Data.FirstOrDefault();
        }
    }
}