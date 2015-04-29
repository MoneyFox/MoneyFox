using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.WindowsPhone.Test.Mocks {
    public class JsonServiceMock : IJsonService {
        public async Task<string> GetJsonFromService(string url) {
            string fileContent;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///TestData/CountryTestData.txt"));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync())) {
                fileContent = await sRead.ReadToEndAsync();
            }
            return fileContent;
        }
    }
}
