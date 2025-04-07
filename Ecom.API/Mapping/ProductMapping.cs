using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entites.Product;

namespace Ecom.API.Mapping
{
    public class ProductMapping :Profile
    {
        public ProductMapping()
        {
            CreateMap<Product,  ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name)).ReverseMap();
            CreateMap<Photo, PhotoDTO>().ReverseMap();


            CreateMap<Product,PhotoDTO>()
                .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.Photos.FirstOrDefault().ImageName)).ReverseMap();

            CreateMap<AddProductDTO, Product>()
                .ForMember(x => x.Photos, x => x.Ignore())
                .ReverseMap();

            CreateMap<UpdateProductDTO, Product>()
                .ForMember(x => x.Photos, x => x.Ignore())
                .ReverseMap();
        }
    }
}
