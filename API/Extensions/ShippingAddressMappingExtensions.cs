using System;
using API.DTOs;
using Core.Entities.OrderAggregate;

namespace API.Extensions;

public static class ShippingAddressMappingExtensions
{
    public static ShippingAddressDTO ToDto(this ShippingAddress shippingAddress)
    {
        
        return new ShippingAddressDTO
        {
            Name = shippingAddress.Name,
            Line1 = shippingAddress.Line1,
            Line2 = shippingAddress.Line2,
            City = shippingAddress.City,
            State = shippingAddress.State,
            Country = shippingAddress.Country,
            PostalCode = shippingAddress.PostalCode
        };
    }

}
