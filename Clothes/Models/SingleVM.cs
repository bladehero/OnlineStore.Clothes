using DAL.Models;
using System.Collections.Generic;

namespace Clothes.Models
{
    public class SingleVM
    {
        public Product Product { get; set; }
        public IEnumerable<ProductPicture> ProductPictures { get; set; }
        public IEnumerable<Product> OtherProducts { get; set; }
    }
}