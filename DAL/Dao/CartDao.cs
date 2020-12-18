using DAL.Models;
using System.Data;

namespace DAL.Dao
{
    public class CartDao : BaseEntityDao<Cart>
    {
        public CartDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.Carts";
        }
    }
}
