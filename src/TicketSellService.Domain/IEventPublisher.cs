namespace TicketSellService.Domain;

public interface IEventPublisher
{
    Task Publish<T>(T @event);
}