using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entites.Product;
namespace Ecom.API.Mapping
{
    public class CategoryMapping :Profile
    {
        public CategoryMapping()
        {
            CreateMap<Photo, CategoryDTO>().ReverseMap();
            CreateMap<Photo, UpdateCategoryDTO>().ReverseMap();
        }
    }
}
