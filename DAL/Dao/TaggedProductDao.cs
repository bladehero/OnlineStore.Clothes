using System.Data;
using DAL.Models;

namespace DAL.Dao
{
    public class TaggedProductDao : BaseEntityDao<TaggedProduct>
    {
        public TaggedProductDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.TaggedProducts";
        }
    }
}
