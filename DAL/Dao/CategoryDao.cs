using DAL.Models;
using System.Data;

namespace DAL.Dao
{
    public class CategoryDao : BaseEntityDao<Category>
    {
        public CategoryDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.Categories";
        }
    }
}
