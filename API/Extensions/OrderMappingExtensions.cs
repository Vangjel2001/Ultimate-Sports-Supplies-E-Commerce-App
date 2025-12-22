using System;
using API.DTOs;
using Core.Entities.OrderAggregate;

namespace API.Extensions;

public static class OrderMappingExtensions
{
   
    public static OrderDTO ToDto(this Order order)
    {
        return new OrderDTO
        {
            Id = order.Id,
            BuyerEmail = order.BuyerEmail,
            OrderDate = order.OrderDate,
            ShippingAddress = order.ShippingAddress.ToDto(),
            PaymentSummary = order.PaymentSummary.ToDto(),
            DeliveryMethod = order.DeliveryMethod.Description,
            ShippingPrice = order.DeliveryMethod.Fee,
            OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
            Subtotal = order.Subtotal,
            Total = order.Total,
            Status = order.Status.ToString(),
            PaymentIntentId = order.PaymentIntentId

        };
    }
    
    public static OrderItemDTO ToDto(this OrderItem orderItem)
    {
        return new OrderItemDTO
        {
            ProductId = orderItem.OrderedItem.ProductId,
            ProductName = orderItem.OrderedItem.ProductName,
            PictureUrl = orderItem.OrderedItem.Picture1Url,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity
        };
    }

}
