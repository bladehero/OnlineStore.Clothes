using DAL.Models;
using System.Data;

namespace DAL.Dao
{
    public class ProductPictureDao : BaseEntityDao<ProductPicture>
    {
        public ProductPictureDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.ProductPictures";
        }
    }
}
