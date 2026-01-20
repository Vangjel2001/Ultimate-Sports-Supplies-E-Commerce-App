using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IPaymentsService
{
    Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId);
}
