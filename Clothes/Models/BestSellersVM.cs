using DAL.Models;
using System.Collections.Generic;

namespace Clothes.Models
{
    public class BestSellersVM
    {
        public IEnumerable<Product> Products { get; set; }
    }
}