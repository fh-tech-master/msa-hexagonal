using TicketSellService.Domain;

namespace TicketSellService.Infrastructure.Adapters;

public class EventPublisher : IEventPublisher
{
    public Task Publish<T>(T @event)
    {
        throw new NotImplementedException();
    }
}