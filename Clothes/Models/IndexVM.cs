using DAL.Models;
using System.Collections.Generic;

namespace Clothes.Models
{
    public class IndexVM
    {
        public IList<Product> Products { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}