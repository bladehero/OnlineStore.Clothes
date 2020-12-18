using DAL.Models;
using System.Data;
using System.Text;

namespace DAL.Dao
{
    public class UserDao : BaseEntityDao<User>
    {
        public UserDao(IDbConnection connection) : base(connection)
        {
            TableName = "dbo.Users";
        }

        public User GetUserByIdHash(int id, string password)
        {
            return FirstOrDefault(x => x.Id == id && x.Password == password);
        }
        public User GetUserByCredentials(string email, string password)
        {
            return FirstOrDefault(x => x.Email == email && x.Password == GetPasswordHash(password));
        }
        public override int Insert(User item)
        {
            item.Password = GetPasswordHash(item.Password);
            return base.Insert(item);
        }
        public override bool Update(User item)
        {
            item.Password = GetPasswordHash(item.Password);
            return base.Update(item);
        }
        private string GetPasswordHash(string password)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
