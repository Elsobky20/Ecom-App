using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.DTO;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await work.productRepository.GetAllAsync(x=>x.Category ,x=>x.Photos);
                var result = mapper.Map<List<ProductDTO>>(products);
                if (products == null || products.Count == 0)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await work.productRepository.GetByIdAsync(id, x => x.Category, x => x.Photos);
                if (product == null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                var result = mapper.Map<ProductDTO>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
