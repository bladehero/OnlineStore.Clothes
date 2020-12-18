using DAL.Models;
using System.Data;

namespace DAL.Dao
{
    public class ProductDao : BaseEntityDao<Product>
    {
        public ProductDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.Products";
        }
    }
}
