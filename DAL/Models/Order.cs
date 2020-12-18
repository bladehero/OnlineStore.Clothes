using System;

namespace DAL.Models
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
