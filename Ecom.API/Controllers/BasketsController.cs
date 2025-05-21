using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.Entites;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : BaseController
    {
        public BasketsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-basket-item/{Id}")]
        public async Task<IActionResult> get(string Id)
        {
            var result = await work.CustomerBasket.GetBasketAsync(Id);
            if (result is null )
            {
                return Ok(new CustomerBasket());
            }
            return Ok(result);
        }
        [HttpPost("update-baske")]
        public async Task<IActionResult> add(CustomerBasket basket )
        {
            var result = await work.CustomerBasket.UpdateBasketAsync(basket);
            return Ok(result);
        }
        [HttpDelete("delete-basket-item/{Id}")]
        public async Task<IActionResult> delete(string Id)
        {
            var result = await work.CustomerBasket.DeleteBasketAsync(Id);
            return result ? Ok(new ResponseAPI(200, "Item Deleted")) 
                : BadRequest(new ResponseAPI(400));
        }
    }
}