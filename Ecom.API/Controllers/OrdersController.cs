using Ecom.API.Helper;
using Ecom.core.DTO;
using Ecom.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create-order")]
        public async Task<IActionResult> Create(OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest(new ResponseAPI(400, "Invalid order data"));
            }
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            //  var email = User.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value; 
            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ResponseAPI(401, "User not authenticated"));
            }
            var order = await _orderService.CreateOrderAsync(orderDTO, buyerEmail);
            if (order == null)
            {
                return BadRequest(new ResponseAPI(400, "Failed to create order"));
            }
            return Ok(order);
        }
        [HttpGet("get-orders-for-user")]
        public async Task<IActionResult> GetOrders()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ResponseAPI(401, "User not authenticated"));
            }
            var orders = await _orderService.GetAllOrdersForUserAsync(buyerEmail);
            if (orders == null || !orders.Any())
            {
                return NotFound(new ResponseAPI(404, "No orders found for this user"));
            }
            return Ok(orders);
        }
        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(buyerEmail))
            {
                return Unauthorized(new ResponseAPI(401, "User not authenticated"));
            }
            var order = await _orderService.GetOrderByIdAsync(id, buyerEmail);
            if (order == null)
            {
                return NotFound(new ResponseAPI(404, "Order not found"));
            }
            return Ok(order);
        }
        [HttpGet("get-delivery")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodAsync();
            if (deliveryMethods == null || !deliveryMethods.Any())
            {
                return NotFound(new ResponseAPI(404, "No delivery methods found"));
            }
            return Ok(deliveryMethods);
        }
    }
}

