namespace TicketSellService.Domain.Model;

/// <summary>
/// Represents a sale where a buyer buys an associated ticket from
/// a seller with an previously arranged price
/// </summary>
public class Sale
{
    public Guid SalesId { get; }
    public Seller Seller { get; }
    public Buyer Buyer { get; }
    public Ticket AssociatedTicket { get; }
    public Price ArrangedPrice { get; }

    public static Sale CreateNewSale(Seller seller, Buyer buyer, Ticket associatedTicket, Price arrangedPrice)
    {
        return new Sale(Guid.NewGuid(), seller, buyer, associatedTicket, arrangedPrice);
    }

    private Sale(Guid salesId, Seller seller, Buyer buyer, Ticket associatedTicket, Price arrangedPrice)
    {
        Seller = seller;
        Buyer = buyer;
        AssociatedTicket = associatedTicket;
        ArrangedPrice = arrangedPrice;
        SalesId = salesId;
    }
}
