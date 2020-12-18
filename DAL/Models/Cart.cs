using System;

namespace DAL.Models
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public string OrderBody { get; set; }
        public DateTime DateTime { get; set; }
    }
}
