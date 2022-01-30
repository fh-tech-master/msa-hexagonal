using TicketSellService.Domain;
using TicketSellService.Domain.Model;
using TicketSellService.Domain.Ports;

namespace TicketSellService.Infrastructure.Adapters;

public class TicketShelfService : ITicketShelfService
{
    public Task<Result> ReserveTicket(Ticket ticket)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SellTicket(Ticket ticket)
    {
        throw new NotImplementedException();
    }
}