namespace TicketSellService.Domain.Model;

/// <summary>
/// A customer that sells a ticket
/// </summary>
/// <param name="CustomerId"></param>
public record Seller(Guid CustomerId);

/// <summary>
/// A customer that buys a ticket
/// </summary>
/// <param name="CustomerId"></param>
public record Buyer(Guid CustomerId);