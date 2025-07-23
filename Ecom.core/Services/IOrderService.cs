using Ecom.core.DTO;
using Ecom.core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Services
{
     public interface IOrderService
    {
        Task<Orders> CreateOrderAsync(OrderDTO orderDTO , string BuyerEmail);
        Task<IReadOnlyList<Orders>> GetAllOrdersForUserAsync(string BuyerEmail);
        Task<Orders> GetOrderByIdAsync(int id, string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();

    }
}
