using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Tests.Mocks
{
    public class CategoryDataAccessMock : IDataAccess<Category>
    {
        public List<Category> CategoryTestList = new List<Category>();

        public void Save(Category itemToSave)
        {
            CategoryTestList.Add(itemToSave);
        }

        public void Delete(Category item)
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