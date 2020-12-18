using DAL.Models;
using System.Data;

namespace DAL.Dao
{
    public class TagDao : BaseEntityDao<Tag>
    {
        public TagDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.Tags";
        }
    }
}
