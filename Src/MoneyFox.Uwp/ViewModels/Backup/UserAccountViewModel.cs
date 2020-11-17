using AutoMapper;
using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Interfaces.Mapping;
using System.IO;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MoneyFox.Uwp.ViewModels.Backup
{
    public class UserAccountViewModel : ViewModelBase, IHaveCustomMapping
    {
        private string name;
        private string email;
        private ImageSource profilePicture;

        /// <summary>
        ///     User's name from Microsoft account.
        /// </summary>
        public string Name {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Users email from Microsoft account.
        /// </summary>
        public string Email {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Users profile photo from Microsoft account.
        /// </summary>
        public ImageSource? ProfilePicture {
            get => profilePicture;
            set
            {
                if(value == profilePicture)
                {
                    return;
                }

                profilePicture = value;
                RaisePropertyChanged();
            }
        }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UserAccount, UserAccountViewModel>()
                         .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom((src, d, ob) =>
                         {
                             if(src.PhotoStream == null)
                             {
                                 return null;
                             }

                             var imageSource = new BitmapImage();
                             imageSource.SetSource(src.PhotoStream.AsRandomAccessStream());
                             return imageSource;
                         }));
        }
    }
}
