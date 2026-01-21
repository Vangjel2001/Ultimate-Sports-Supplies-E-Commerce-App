using System;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces;

public interface IOrdersRepository : IRepository<Order>
{
    Task<List<Order>> GetAllOrdersForUser(string userEmail);
    Task<Order?> GetOrderForUserByOrderId(string userEmail, int orderId);
    Task<Order?> GetOrderByPaymentIntentId(string paymentIntentId);
}
