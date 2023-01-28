namespace MoneyFox.Core.Features.DbBackup;

using System.IO;
using System.Threading.Tasks;

public interface IOneDriveProfileService
{
    Task<UserAccountDto> GetUserAccountAsync();

    Task<Stream> GetProfilePictureAsync();
}
