using TicketSellService.Domain.Model;

namespace TicketSellService.Domain.Services;

/// <summary>
/// Application layer interface when accessing ticket sales
/// </summary>
public interface ITicketSellService
{
    /// <summary>
    /// Sells a ticket
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<Result> SellTicket(SellTicketCommand command);
}

/// <summary>
/// Holds information for selling a ticket
/// </summary>
/// <param name="Buyer"></param>
/// <param name="Seller"></param>
/// <param name="Ticket"></param>
/// <param name="Price"></param>
public record SellTicketCommand(Buyer Buyer, Seller Seller, TicketToSell Ticket, Price Price);

/// <summary>
/// A ticket that should be sold
/// </summary>
/// <param name="TicketId"></param>
public record TicketToSell(Guid TicketId);
