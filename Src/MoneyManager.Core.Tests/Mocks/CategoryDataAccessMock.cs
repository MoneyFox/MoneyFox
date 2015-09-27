using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Mocks
{
    public class CategoryDataAccessMock : IDataAccess<Category>
    {
        public List<Category> CategoryTestList = new List<Category>();

        public void SaveItem(Category itemToSave)
        {
            CategoryTestList.Add(itemToSave);
        }

        public void DeleteItem(Category item)
        {
            if (CategoryTestList.Contains(item))
            {
                CategoryTestList.Remove(item);
            }
        }

        public List<Category> LoadList(Expression<Func<Category, bool>> filter = null)
        {
            return new List<Category>();
        }
    }
}