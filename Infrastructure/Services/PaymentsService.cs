using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

public class PaymentsService(IConfiguration config, ICartService cartService, IRepository<DeliveryMethod> deliveryMethodsRepository, IProductsRepository productsRepository) : IPaymentsService
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

        var cart = await cartService.GetCartAsync(cartId);

        if (cart == null)
        {
            return null;
        }

        var shippingFee = 0m;

        if(cart.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await deliveryMethodsRepository.GetByIdAsync((int)cart.DeliveryMethodId);

            if (deliveryMethod == null)
            {
                return null;
            }

            shippingFee = deliveryMethod.Fee;
        }

        foreach (var item in cart.Items)
        {
            var product = await productsRepository.GetByIdAsync(item.ProductId);

            if (product == null)
            {
                return null;
            }

            if(item.Price != product.Price)
            {
                item.Price = product.Price;
            }
        }

        var paymentIntentService = new PaymentIntentService();
        PaymentIntent? intent = null;

        if (string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var paymentIntentOptions = new PaymentIntentCreateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                    + (long)shippingFee * 100,
                Currency = "usd",
                PaymentMethodTypes = ["card"]
            };

            intent = await paymentIntentService.CreateAsync(paymentIntentOptions);
            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var paymentIntentUpdateOptions = new PaymentIntentUpdateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                    + (long)shippingFee * 100
            };
            intent = await paymentIntentService.UpdateAsync(cart.PaymentIntentId, paymentIntentUpdateOptions);
        }

        await cartService.SetCartAsync(cart);

        return cart;
    }
}
