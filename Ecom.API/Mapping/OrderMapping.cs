using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entites.Order;

namespace Ecom.API.Mapping
{
    public class OrderMapping : Profile
    {
        
        public OrderMapping()
        {
            CreateMap<ShippingAddress, ShipAddressDTO>().ReverseMap();
        }

       
    }
}
