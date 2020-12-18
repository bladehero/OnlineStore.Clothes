namespace DAL.Models
{
    public class TaggedProduct : BaseEntity
    {
        public int ProductId { get; set; }
        public int TagId { get; set; }
    }
}
