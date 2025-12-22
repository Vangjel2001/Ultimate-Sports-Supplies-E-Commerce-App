using System;
using API.DTOs;
using Core.Entities.OrderAggregate;

namespace API.Extensions;

public static class PaymentSummaryMappingExtensions
{
   public static PaymentSummaryDTO ToDto(this PaymentSummary paymentSummary)
    {
        return new PaymentSummaryDTO
        {
           Last4 = paymentSummary.Last4,
           ExpMonth = paymentSummary.ExpMonth,
           ExpYear = paymentSummary.ExpYear,
           Brand = paymentSummary.Brand
        };
    }
}
