using DAL.Dao;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UnitOfWork : IDisposable
    {
        private IDbConnection _connection => new SqlConnection(ConnectionString);

        public string ConnectionString { get; set; }

        public UnitOfWork() { }
        public UnitOfWork(string connection) => ConnectionString = connection;

        public CartDao Carts => new CartDao(_connection);
        public CategoryDao Categories => new CategoryDao(_connection);
        public OrderDao Orders => new OrderDao(_connection);
        public OrderProductDao OrderProducts => new OrderProductDao(_connection);
        public ProductDao Products => new ProductDao(_connection);
        public TagDao Tags => new TagDao(_connection);
        public TaggedProductDao TaggedProducts => new TaggedProductDao(_connection);
        public UserDao Users => new UserDao(_connection);
        public ProductPictureDao ProductPictures => new ProductPictureDao(_connection);
        public MessageDao Messages => new MessageDao(_connection);
        public NewsletterDao Newsletters => new NewsletterDao(_connection);

        public void Dispose()
        {
            Carts.Dispose();
            Categories.Dispose();
            Orders.Dispose();
            OrderProducts.Dispose();
            Products.Dispose();
            Tags.Dispose();
            TaggedProducts.Dispose();
            ProductPictures.Dispose();
            Messages.Dispose();
            Newsletters.Dispose();
        }
    }
}
