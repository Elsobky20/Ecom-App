using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.DTO;
using Ecom.core.Entites.Product;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await work.categoryRepository.GetAllAsync();
                if (categories == null || categories.Count == 0)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(categories);

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
                var category = await work.categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> Add([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                var category = mapper.Map<Photo>(categoryDTO);

                await work.categoryRepository.AddAsync(category);
                return Ok(new ResponseAPI(200 , "Item has been Added"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                var category = await work.categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
                await work.categoryRepository.DeleteAsync(id);
                return Ok(new ResponseAPI(200, "Item has been Deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-category")]
        public async Task<IActionResult> Update(UpdateCategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                {
                    return BadRequest("Category is null");
                }
                var category = await work.categoryRepository.GetByIdAsync(categoryDTO.Id);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
               mapper.Map(categoryDTO, category);
                await work.categoryRepository.UpdateAsync(category);
                return Ok(new ResponseAPI(200, "Item has been Updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         
    }
}
