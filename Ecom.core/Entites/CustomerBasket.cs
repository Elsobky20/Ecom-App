using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Entites
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
            
        }
        public CustomerBasket(string id )
        {
            this.Id = id;
        }
        public string Id { set; get; }
        public List<BasketItem> basketItem { set; get; } = new List<BasketItem>();
    }
}
