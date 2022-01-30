using TicketSellService.Domain.Model;

namespace TicketSellService.Domain.Ports;

/// <summary>
/// Access to a ticket shelf service
/// </summary>
public interface ITicketShelfService
{
    /// <summary>
    /// Tries to reserve a given <paramref name="ticket"/>
    /// </summary>
    /// <param name="ticket"></param>
    /// <returns></returns>
    Task<Result> ReserveTicket(Ticket ticket);
    
    /// <summary>
    /// Tries to sell a given <paramref name="ticket"/>
    /// </summary>
    /// <param name="ticket"></param>
    /// <returns></returns>
    Task<Result> SellTicket(Ticket ticket);
}