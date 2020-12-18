using System.Data;
using DAL.Models;

namespace DAL.Dao
{
    public class OrderProductDao : BaseEntityDao<OrderProduct>
    {
        public OrderProductDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.OrderProducts";
        }
    }
}
