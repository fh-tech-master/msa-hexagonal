namespace TicketSellService.Domain.Model;

/// <summary>
/// A ticket that can be sold
/// </summary>
/// <param name="TicketId"></param>
/// <param name="CurrentState"></param>
public record Ticket(Guid TicketId, TicketState CurrentState);

/// <summary>
/// Represents a <see cref="Ticket"/>s state
/// </summary>
public enum TicketState
{
    Open,
    Reserved,
    Sold
}
