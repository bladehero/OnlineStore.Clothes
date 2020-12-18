using Dapper;
using DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL.Dao
{
    public class BaseEntityDao<T> : IEnumerable<T>, IEnumerable, IDisposable where T : BaseEntity
    {
        public IDbConnection Connection { get; set; }
        protected string TableName { get; set; }

        public BaseEntityDao(IDbConnection connection) => Connection = connection;

        public IEnumerator GetEnumerator() => FindAll().GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => FindAll().GetEnumerator();
        public void Dispose() => Connection.Dispose();

        public int Count() => Connection.QueryFirstOrDefault<int>($"select count(Id) from {TableName}");

        public virtual IEnumerable<T> FindAll() => Connection.Query<T>($"select * from {TableName}");
        public virtual IEnumerable<T> Find(Func<T, bool> predicate) => Connection.Query<T>($"select * from {TableName}").Where(predicate);
        public virtual T FirstOrDefault(Func<T, bool> predicate) => Connection.Query<T>($"select * from {TableName}").FirstOrDefault(predicate);
                
        public virtual IEnumerable<T> Random(int count)
        {
            var items = FindAll();
            if (count > Count())
            {
                return items;
            }
            else
            {
                Random random = new Random();
                var itemList = items.ToList();
                var randomList = new List<T>(count);

                for (int i = 0; i < count;)
                {
                    int next = random.Next(items.Count());
                    if (!randomList.Contains(itemList[next]))
                    {
                        randomList.Add(itemList[next]);
                        i++;
                    }
                }
                return randomList;
            }
        }
        public virtual T FirstOrDefaultRandom()
        {
            var count = Count();
            if (count == 0)
            {
                return null;
            }
            Random random = new Random();
            return FindAll().ToList()[random.Next(count)];
        }
                
        public virtual int Insert(T item)
        {
            var properties = item.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            var sql = $"insert into {TableName} ({string.Join(",", properties.Select(x => x.Name))}) values ({string.Join(",", properties.Select(x => "@" + x.Name))}) SELECT CAST(SCOPE_IDENTITY() as int)";
            return item.Id = Connection.Query<int>(sql, item).Single();
        }
        public virtual bool Update(T item)
        {
            var properties = item.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            var sql = $"update {TableName} set {string.Join(",", properties.Select(x => x.Name + " = @" + x.Name))} where Id = @Id";
            return Connection.Execute(sql, item) > 0;
        }
        public virtual bool Delete(int id) => Connection.Execute($"update {TableName} set IsDeleted = 1 where Id = {id}") > 0;
        public virtual bool Delete(T item) => Delete(item.Id);
        public virtual bool Merge(T item) => item?.Id == 0 ? Insert(item) > 0 : Update(item);
    }
}
