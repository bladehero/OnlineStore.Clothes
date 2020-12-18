using DAL.Models;
using System.Collections.Generic;

namespace Clothes.Models
{
    public class CategoriesVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public int? SelectedCategory { get; set; }
    }
}