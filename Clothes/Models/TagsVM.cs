using DAL.Models;
using System.Collections.Generic;

namespace Clothes.Models
{
    public class TagsVM
    {
        public IEnumerable<Tag> Tags { get; set; }
        public int? SelectedTag { get; set; }
    }
}