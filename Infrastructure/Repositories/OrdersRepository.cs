using System;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrdersRepository(Data.ApplicationContext context) : Repository<Order>(context), IOrdersRepository
{
    public Task<List<Order>> GetAllOrdersForUser(string userEmail)
    {
        var query = context.Orders.AsQueryable();

        query = query.Where(o => o.BuyerEmail == userEmail);

        query = query.Include(o => o.OrderItems);
        query = query.Include(o => o.DeliveryMethod);
        query = query.OrderByDescending(o => o.OrderDate);

        var orders = query.ToListAsync();

        return orders;
    }

    public Task<Order?> GetOrderByPaymentIntentId(string paymentIntentId)
    {
        var query = context.Orders.AsQueryable();

        query = query.Where(o => o.PaymentIntentId == paymentIntentId);

        query = query.Include(o => o.OrderItems);
        query = query.Include(o => o.DeliveryMethod);

        var order = query.FirstOrDefaultAsync();

        return order;
    }

    public Task<Order?> GetOrderForUserByOrderId(string userEmail, int orderId)
    {
        var query = context.Orders.AsQueryable();

        query = query.Where(o => o.BuyerEmail == userEmail && o.Id == orderId);

        query = query.Include(o => o.OrderItems);
        query = query.Include(o => o.DeliveryMethod);

        var order = query.FirstOrDefaultAsync();

        return order;
    }
}
