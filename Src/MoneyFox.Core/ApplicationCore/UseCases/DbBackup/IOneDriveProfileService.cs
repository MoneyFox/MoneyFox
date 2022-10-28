namespace MoneyFox.Core.ApplicationCore.UseCases.DbBackup;

using System.IO;
using System.Threading.Tasks;

public interface IOneDriveProfileService
{
    Task<UserAccountDto> GetUserAccountAsync();

    Task<Stream> GetProfilePictureAsync();
}
