using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PaymentsController(IRepository<DeliveryMethod> deliveryMethodsRepository, IPaymentsService paymentsService) : BaseApiController
{
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
}
