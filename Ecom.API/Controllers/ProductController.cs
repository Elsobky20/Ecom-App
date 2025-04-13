using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.DTO;
using Ecom.core.Entites.Product;
using Ecom.core.Interfaces;
using Ecom.core.Sharing;
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
        public async Task<IActionResult> GetAll([FromQuery] ProductParams productParams)
        {
            try
            {
               var products = await work.productRepository.GetAllAsync(productParams);
                var totalCount = await work.productRepository.CountAsync();
                return Ok(new Pagination<ProductDTO>(productParams.PageNumber , productParams.PageSize, totalCount, products.ToList()));
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
        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDTO productDTO)
        {
            try
            {
               await work.productRepository.AddAsync(productDTO);
                return Ok(new ResponseAPI(200));

            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("Update-Product")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDTO productDTO)
        {
            try
            {
                await work.productRepository.UpdateAsync(productDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete-Product/{Id}")] 
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await work.productRepository.GetByIdAsync(id , x=>x.Photos , x=>x.Category);
                await work.productRepository.DeleteAsync(product);
                return Ok(new ResponseAPI(200));

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


    }
}
