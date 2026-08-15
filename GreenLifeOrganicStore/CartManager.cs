using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLifeOrganicStore
{
    public class CartManager
    {
        public static List<CartItem> Cart = new List<CartItem>();

        public static int GetCartCount()
        {
            return Cart.Sum(i => i.Quantity);
        }
    }
}