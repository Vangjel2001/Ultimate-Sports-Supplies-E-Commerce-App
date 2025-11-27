namespace Core.Entities.OrderAggregate;

public enum OrderStatus
{
    PaymentPending,
    PaymentSucceeded,
    PaymentFailed,
    PaymentMismatch
}
