namespace TicketSellService.Domain.Model;

/// <summary>
/// A monetary value with <paramref name="PriceNetCents"/> (smallest unit in the given currency)
/// and a valid ISO <paramref name="CurrencyCode"/>
/// </summary>
/// <param name="PriceNetCents"></param>
/// <param name="CurrencyCode"></param>
public record Price(int PriceNetCents, string CurrencyCode);