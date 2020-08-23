using GalaSoft.MvvmLight;
using MoneyFox.Application.Common.Interfaces.Mapping;
using MoneyFox.Domain.Entities;
using System;

namespace MoneyFox.Uwp.ViewModels
{
    public class CategoryViewModel : ViewModelBase, IMapFrom<Category>
    {
        private int id;
        private string name = "";
        private string note = "";
        private DateTime creationTime;
        private DateTime modificationDate;

        public int Id
        {
            get => id;
            set
            {
                if(id == value)
                    return;
                id = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if(name == value)
                    return;
                name = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Additional details about the CategoryViewModel
        /// </summary>
        public string Note
        {
            get => note;
            set
            {
                if(note == value)
                    return;
                note = value;
                RaisePropertyChanged();
            }
        }

        public DateTime CreationTime
        {
            get => creationTime;
            set
            {
                if(creationTime == value)
                    return;
                creationTime = value;
                RaisePropertyChanged();
            }
        }

        public DateTime ModificationDate
        {
            get => modificationDate;
            set
            {
                if(modificationDate == value)
                    return;
                modificationDate = value;
                RaisePropertyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            return (obj as CategoryViewModel)?.Id == Id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 7;
                hashCode = (hashCode * 397) ^ id.GetHashCode();

                return hashCode;
            }
        }
    }
}
