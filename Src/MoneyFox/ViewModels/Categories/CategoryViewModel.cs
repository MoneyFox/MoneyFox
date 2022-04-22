namespace MoneyFox.ViewModels.Categories
{

    using System;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Core.Aggregates;
    using Core.Aggregates.CategoryAggregate;
    using Core.Common.Interfaces.Mapping;

    public class CategoryViewModel : ObservableObject, IMapFrom<Category>
    {
        private DateTime creationTime;
        private int id;
        private DateTime modificationDate;
        private string name = "";
        private string note = "";
        private bool requireNote;

        public int Id
        {
            get => id;

            set
            {
                if (id == value)
                {
                    return;
                }

                id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;

            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;
                OnPropertyChanged();
            }
        }

        public bool RequireNote
        {
            get => requireNote;

            set
            {
                if (requireNote == value)
                {
                    return;
                }

                requireNote = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreationTime
        {
            get => creationTime;

            set
            {
                if (creationTime == value)
                {
                    return;
                }

                creationTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime ModificationDate
        {
            get => modificationDate;

            set
            {
                if (modificationDate == value)
                {
                    return;
                }

                modificationDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Additional details about the CategoryViewModel
        /// </summary>
        public string Note
        {
            get => note;

            set
            {
                if (note == value)
                {
                    return;
                }

                note = value;
                OnPropertyChanged();
            }
        }
    }

}
