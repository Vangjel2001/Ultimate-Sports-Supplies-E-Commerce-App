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

    public static PaymentSummary ToEntity(this PaymentSummaryDTO paymentSummaryDTO)
    {
        if (paymentSummaryDTO == null)
        {
            throw new ArgumentNullException(nameof(paymentSummaryDTO));
        }

        return new PaymentSummary
        {
            Last4 = paymentSummaryDTO.Last4,
            ExpMonth = paymentSummaryDTO.ExpMonth,
            ExpYear = paymentSummaryDTO.ExpYear,
            Brand = paymentSummaryDTO.Brand
        };
    }
}
