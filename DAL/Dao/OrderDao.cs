using DAL.Models;
using System.Data;

namespace DAL.Dao
{
    public class OrderDao : BaseEntityDao<Order>
    {
        public OrderDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.Orders";
        }
    }
}
