using System.Threading.Tasks;

namespace MoneyManager.Foundation.OperationContracts {
    public interface IJsonService {

        Task<string> GetJsonFromService();
    }
}
