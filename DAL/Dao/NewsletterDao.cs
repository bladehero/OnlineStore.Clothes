using DAL.Models;
using System.Data;

namespace DAL.Dao
{
    public class NewsletterDao : BaseEntityDao<Newsletter>
    {
        public NewsletterDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.Newsletters";
        }
    }
}
