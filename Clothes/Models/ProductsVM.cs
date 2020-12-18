using DAL.Models;
using System.Collections.Generic;

namespace Clothes.Models
{
    public class ProductsVM
    {
        public IEnumerable<Product> Products { get; set; }
        public Category SelectedCategory { get; set; }
        public Tag SelectedTag { get; set; }
        public string SearchString { get; set; }
    }
}