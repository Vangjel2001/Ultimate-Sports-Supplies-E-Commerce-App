using System;
using API.Extensions;
using API.SignalR;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;

namespace API.Controllers;

public class PaymentsController(IRepository<DeliveryMethod> deliveryMethodsRepository, IOrdersRepository ordersRepository, IPaymentsService paymentsService, ILogger<PaymentsController> logger, IConfiguration config, IHubContext<NotificationsHub> hubContext) : BaseApiController
{
    private readonly string _whSecret = config["StripeSettings:WhSecret"]!;

    [Authorize]
    [HttpPost("{cartId}")]
    public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
    {
        var cart = await paymentsService.CreateOrUpdatePaymentIntent(cartId);

        if(cart == null)
        {
            return BadRequest("Problem with cart");
        }
        else
        {
            return Ok(cart);
        }
    }

    [HttpGet("delivery-methods")]
    public async Task<ActionResult<IList<DeliveryMethod>>> GetDeliveryMethods()
    {
        return Ok(await deliveryMethodsRepository.GetAllAsync());
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = ConstructStripeEvent(json);

            if(stripeEvent.Data.Object is not PaymentIntent intent)
            {
                return BadRequest("Invalid event data");
            }

            await HandlePaymentIntentSucceeded(intent);

            return Ok();            
        }
        catch (StripeException ex)
        {
            logger.LogError(ex, "Stripe webhook error");
            return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
    {
        if (intent.Status == "succeeded")
        {
            var order = await ordersRepository.GetOrderByPaymentIntentId(intent.Id);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            if((long)order.Total * 100 != intent.Amount)
            {
                order.Status = OrderStatus.PaymentMismatch;
            }
            else
            {
                order.Status = OrderStatus.PaymentSucceeded;
            }

            var connectionId = NotificationsHub.GetConnectionIdByEmail(order.BuyerEmail);

            if (string.IsNullOrEmpty(connectionId) == false)
            {
                await hubContext.Clients.Client(connectionId).SendAsync("OrderCompleteNotification", order.ToDto());
            }
        }
    }

    private Event ConstructStripeEvent(string json)
    {
        try
        {
            return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to construct the Stripe event");
            throw new StripeException("Invalid signature");
        }
    }
}
