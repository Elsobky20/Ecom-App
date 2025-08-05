using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entites.Order;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, AppDbContext appDbContext, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._context = appDbContext;
            _mapper = mapper;
        }
        public async Task<Orders> CreateOrderAsync(OrderDTO orderDTO, string BuyerEmail)
        {
            var basket = await _unitOfWork.CustomerBasket.GetBasketAsync(orderDTO.basketId);
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.basketItem)
            {
                var product = await _unitOfWork.productRepository.GetByIdAsync(item.Id);
                var orderItem = new OrderItem
                {
                    ProductItemId = product.Id ,
                    MainImage = item.Image,
                    ProductName = product.Name,
                    Quntity = item.Quentity,
                    Price = item.Price
                };
                orderItems.Add(orderItem);
            }
            var deliveryMethod = await _context.DeliveryMethods.FirstOrDefaultAsync(x => x.Id == orderDTO.deliveryMethodId);
            var subTotal = orderItems.Sum(x => x.Price * x.Quntity); 
            var ship= _mapper.Map<ShippingAddress>(orderDTO.shipAddress);
            var Order = new Orders
            {
                BuyerEmail = BuyerEmail,
                SubTotal = subTotal,
                ShippingAddress = ship,
                DeliveryMethod = deliveryMethod,
                OrderItems = orderItems
            };
            await _context.Orders.AddAsync(Order);
             await _context.SaveChangesAsync() ;
            await _unitOfWork.CustomerBasket.DeleteBasketAsync(orderDTO.basketId);
            return Order;
        }

        public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var orders =await _context.Orders
                .Where(o => o.BuyerEmail == BuyerEmail)
                .Include(x => x.OrderItems)
                .Include(x => x.DeliveryMethod)
                .AsNoTracking()
                .ToListAsync();
            var result = _mapper.Map<IReadOnlyList< OrderToReturnDTO>>(orders);
            return result;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
            =>await _context.DeliveryMethods.AsNoTracking().ToListAsync();

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(int id, string BuyerEmail)
        {
            var order = await _context.Orders.Where(o => o.Id == id && o.BuyerEmail == BuyerEmail)
                .Include(x=>x.OrderItems)
                .Include(x=>x.DeliveryMethod)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<OrderToReturnDTO>(order);
            return result;
        }
    }
}
