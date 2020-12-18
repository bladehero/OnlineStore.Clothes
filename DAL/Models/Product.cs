namespace DAL.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ReviewCount { get; set; }
        public int Mark { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
    }
}
