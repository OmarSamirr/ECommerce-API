using AutoMapper;
using Domain.Models.Orders;
using Domain.Models.Products;
using Microsoft.Extensions.Configuration;
using Shared.Authentication;
using Shared.DataTransferObjects.Orders;
using Shared.DataTransferObjects.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAddress, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom<PictureUrlResolver>());

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.DeliveryMethod,
                options => options.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total,
                options => options.MapFrom(src => src.DeliveryMethod.Cost + src.SubTotal));

            CreateMap<DeliveryMethod, DeliveryMethodResponse>();

        }
    }
    public class PictureUrlResolver(IConfiguration _configuration) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(source.PictureUrl))
                return $"{_configuration["BaseUrl"]}{source.PictureUrl}";
            return string.Empty;
        }
    }
}
