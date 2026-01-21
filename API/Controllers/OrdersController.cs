using System;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController(ICartService cartService, IOrdersRepository ordersRepository, IProductsRepository productsRepository, 
IRepository<DeliveryMethod> deliveryMethodsRepository) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDTO createOrderDTO)
    {
        var email = User.GetEmail();

        var cart = await cartService.GetCartAsync(createOrderDTO.CartId);

        if (cart == null)
        {
            return BadRequest("Cart not found");
        }

        if (cart.PaymentIntentId == null)
        {
            return BadRequest("This order has no payment intent");
        }

        var orderItems = new List<OrderItem>();

        foreach (var cartItem in cart.CartItems)
        {
            var product = await productsRepository.GetByIdAsync(cartItem.ProductId);

            if (product == null)
            {
                return BadRequest("There is a problem with the order");
            }

            var orderedProductItem = new OrderedProductItem
            {
                ProductId = cartItem.ProductId,
                ProductName = cartItem.ProductName,
                Picture1Url = cartItem.PictureUrl
            };

            var orderItem = new OrderItem
            {
                OrderedItem = orderedProductItem,
                Price = product.Price,
                Quantity = cartItem.Quantity
            };

            orderItems.Add(orderItem);
        }

        var deliveryMethod = await deliveryMethodsRepository.GetByIdAsync(createOrderDTO.DeliveryMethodId);

        if (deliveryMethod == null)
        {
            return BadRequest("No delivery method was selected");
        }

        var subtotal = orderItems.Sum(x => x.Price * x.Quantity);

        var order = new Order
        {
            OrderItems = orderItems,
            DeliveryMethod = deliveryMethod,
            ShippingAddress = createOrderDTO.ShippingAddressDTO.ToEntity(),
            Subtotal = subtotal,
            PaymentSummary = createOrderDTO.PaymentSummaryDTO.ToEntity(),
            PaymentIntentId = cart.PaymentIntentId,
            BuyerEmail = email,
            Total = subtotal + deliveryMethod.Fee
        };

        ordersRepository.Add(order);

        if (await ordersRepository.Complete())
        {
            return order;
        }
        else
        {
            return BadRequest("A problem occurred while creating the order");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDTO>>> GetOrdersForUser()
    {
        var orders = await ordersRepository.GetAllOrdersForUser(User.GetEmail());

        var ordersToReturn = orders.Select(o => o.ToDto()).ToList();

        return Ok(ordersToReturn);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
    {
        var order = await ordersRepository.GetOrderForUserByOrderId(User.GetEmail(), id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order.ToDto());
    }
}
