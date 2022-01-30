using TicketSellService.Domain.Model;
using TicketSellService.Domain.Model.Events;
using TicketSellService.Domain.Ports;

namespace TicketSellService.Domain.Services;

public class DefaultTicketSellService : ITicketSellService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketShelfService _ticketShelfService;
    private readonly IAccountingService _accountingService;
    private readonly IEventPublisher _eventPublisher;

    public DefaultTicketSellService(
        ITicketRepository ticketRepository, 
        ITicketShelfService ticketShelfService, 
        IAccountingService accountingService,
        IEventPublisher eventPublisher)
    {
        _ticketRepository = ticketRepository;
        _ticketShelfService = ticketShelfService;
        _accountingService = accountingService;
        _eventPublisher = eventPublisher;
    }
    
    public async Task<Result> SellTicket(SellTicketCommand command)
    {
        await _eventPublisher.Publish(new SaleStartedEvent(command.Ticket.TicketId));
        var ticket = await FetchValidTicket(command.Ticket.TicketId);
        if (ticket is not Some<Ticket> actualTicket)
        {
            return new Error("Ticket cannot be sold");
        }

        var reserveTicketResult = await ReserveTicket(actualTicket);
        if (reserveTicketResult is Error)
        {
            return new Error("Unable to reserve ticket");
        }

        var moneyTransferResult = await TransferMoney(command);
        if (moneyTransferResult is Error)
        {
            return new Error("Unable to transfer money");
        }

        var sellTicketResult = await SellTicket(actualTicket);
        if (sellTicketResult is Error)
        {
            await _accountingService.RollBackTransfer();
            return new Error("Unable to sell ticket");
        }

        var sale = Sale.CreateNewSale(command.Seller, command.Buyer, actualTicket.Value, command.Price);

        // When publishing this event, handlers for social media platform and the metrics would listen on these event
        // We would calculate the metrics for successful/failed sales by tracking SaleStartedEvent and SaleSucceededEvent events
        await _eventPublisher.Publish(new SaleSucceededEvent(sale));
        return new Ok();
    }

    private async Task<Optional<Ticket>> FetchValidTicket(Guid ticketId)
    {
        var ticket = await _ticketRepository.FindById(ticketId);

        if (ticket is not Some<Ticket> actualTicket)
        {
            return new None<Ticket>();
        }
        
        if (actualTicket.Value.CurrentState is not TicketState.Open)
        {
            return new None<Ticket>();
        }

        return new Some<Ticket>(actualTicket.Value);
    }
    
    private async Task<Result> ReserveTicket(Some<Ticket> actualTicket)
    {
        var reserveTicketResult = await _ticketShelfService.ReserveTicket(actualTicket.Value);
        return reserveTicketResult;
    }
    
    private async Task<Result> TransferMoney(SellTicketCommand command)
    {
        var moneyTransferResult = await _accountingService.TransferMoney(command.Buyer, command.Seller, command.Price);
        return moneyTransferResult;
    }
    
    private async Task<Result> SellTicket(Some<Ticket> actualTicket)
    {
        var sellTicketResult = await _ticketShelfService.SellTicket(actualTicket.Value);
        return sellTicketResult;
    }
}