using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLifeOrganicStore
{
    public class CartItem
    {
        public Product Product { get; set; }
        
        public string ProductName
        {
            get { return Product.Name; }
        }
        public int Quantity { get; set; }
        public decimal SubTotal
        {
            get { return Product.FinalPrice * Quantity; }
        }
    }
}
