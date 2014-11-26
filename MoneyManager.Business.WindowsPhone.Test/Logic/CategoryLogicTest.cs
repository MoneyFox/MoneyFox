using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess;
using MoneyManager.DataAccess.Model;

namespace MoneyManager.Business.WindowsPhone.Test.Logic
{
  [TestClass]
  public class CategoryLogicTest{

    [TestInitialize]
    public void InitTest(){
      new ViewModelLocator();

        DatabaseLogic.CreateDatabase();
    }

    [TestMethod]
    public void DeleteCategoryTest(){
      var category1 = new Category {
        Name = "Einkaufen"
      };
      var category2 = new Category {
        Name = "Sparen"
      };
      var category3 = new Category {
        Name = "Schule"
      };

      using(var db = SqlConnectionFactory.GetSqlConnection()){
        db.Insert(category1);
        db.Insert(category2);
        db.Insert(category3);
      }

      CategoryLogic.DeleteCategory(category1, true);
      CategoryLogic.DeleteCategory(category3, true);

      using(var db = SqlConnectionFactory.GetSqlConnection()){
        Assert.IsFalse(db.Table<Category>().Any(x => x.Id == category1.Id));
        Assert.IsTrue(db.Table<Category>().Any(x => x.Id == category2.Id));
        Assert.IsFalse(db.Table<Category>().Any(x => x.Id == category3.Id));
      }
    }
  }
}
