using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Tests.Mocks
{
    public class CategoryDataAccessMock : IDataAccess<Category>
    {
        public List<Category> CategoryTestList = new List<Category>();

        public bool SaveItem(Category itemToSave)
        {
            CategoryTestList.Add(itemToSave);
            return true;
        }

        public bool DeleteItem(Category item)
        {
            if (CategoryTestList.Contains(item))
            {
                CategoryTestList.Remove(item);
            }
            return true;
        }

        public List<Category> LoadList(Expression<Func<Category, bool>> filter = null) => CategoryTestList;
    }
}