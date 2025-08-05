using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entites;
using Ecom.core.Entites.Order;
using StackExchange.Redis;

namespace Ecom.API.Mapping
{
    public class OrderMapping : Profile
    {

        public OrderMapping()
        {
            CreateMap<ShippingAddress, ShipAddressDTO>().ReverseMap();

            CreateMap<Orders, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, o => o.
                MapFrom(s => s.DeliveryMethod.Name))
            .ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();

            CreateMap<Address,ShipAddressDTO>().ReverseMap();


        }
    }
}
