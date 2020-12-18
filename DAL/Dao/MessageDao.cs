using DAL.Models;
using System.Data;

namespace DAL.Dao
{
    public class MessageDao : BaseEntityDao<Message>
    {
        public MessageDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.Messages";
        }
    }
}
