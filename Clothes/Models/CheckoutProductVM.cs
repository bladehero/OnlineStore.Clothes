using DAL.Models;

namespace Clothes.Models
{
    public class CheckoutProductVM
    {
        public Product Product { get; set; }
        public int Quantity { get; set; } 
    }
}