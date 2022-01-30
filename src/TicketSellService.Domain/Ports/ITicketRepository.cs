using TicketSellService.Domain.Model;

namespace TicketSellService.Domain.Ports;

/// <summary>
/// Access to <see cref="Ticket"/>s
/// </summary>
public interface ITicketRepository
{
    /// <summary>
    /// Finds a ticket by its <paramref name="ticketId"/>
    /// </summary>
    /// <param name="ticketId"></param>
    /// <returns></returns>
    Task<Optional<Ticket>> FindById(Guid ticketId);
}