using TicketSellService.Domain;
using TicketSellService.Domain.Model;
using TicketSellService.Domain.Ports;

namespace TicketSellService.Infrastructure.Adapters;

public class TicketRepository : ITicketRepository
{
    public Task<Optional<Ticket>> FindById(Guid ticketId)
    {
        throw new NotImplementedException();
    }
}