using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TicketSellService.Domain;
using TicketSellService.Domain.Model;
using TicketSellService.Domain.Ports;
using TicketSellService.Domain.Services;
using Xunit;

namespace TicketSellService.Domain.Test;

public class TicketSellServiceTests
{
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly Mock<IAccountingService> _accountingService;
    private readonly Mock<ITicketShelfService> _ticketShelfService;
    private readonly ITicketSellService _sut;

    private readonly Seller _seller;
    private readonly Buyer _buyer;
    private readonly TicketToSell _ticketToSell;
    private readonly Price _price;

    public TicketSellServiceTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _accountingService = new Mock<IAccountingService>();
        _ticketShelfService = new Mock<ITicketShelfService>();
        var eventPublisher = new Mock<IEventPublisher>();
        _sut = new DefaultTicketSellService(_ticketRepository.Object, _ticketShelfService.Object, _accountingService.Object, eventPublisher.Object);
        
        _seller = new Seller(Guid.NewGuid());
        _buyer = new Buyer(Guid.NewGuid());
        _ticketToSell = new TicketToSell(Guid.NewGuid());
        _price = new Price(500, "EUR");
    }
    
    [Fact]
    public async Task ItShallSellTicket()
    {
        // given
        var ticket = new Ticket(_ticketToSell.TicketId, TicketState.Open);

        var command = new SellTicketCommand(_buyer, _seller, _ticketToSell, _price);

        _ticketRepository.Setup(r => r.FindById(_ticketToSell.TicketId))
            .ReturnsAsync(() => new Some<Ticket>(ticket));

        _ticketShelfService.Setup(s => s.ReserveTicket(ticket))
            .ReturnsAsync(() => new Ok());
        
        _accountingService.Setup(s => s.TransferMoney(_buyer, _seller, _price))
            .ReturnsAsync(() => new Ok());

        _ticketShelfService.Setup(s => s.SellTicket(ticket))
            .ReturnsAsync(() => new Ok());

        // when
        var result = await _sut.SellTicket(command);
        
        // then
        result.Should().BeOfType<Ok>();
    }
    
    [Fact]
    public async Task ItShallNotSellAlreadySoldTicket()
    {
        // given
        var ticket = new Ticket(_ticketToSell.TicketId, TicketState.Sold);

        var command = new SellTicketCommand(_buyer, _seller, _ticketToSell, _price);

        _ticketRepository.Setup(r => r.FindById(_ticketToSell.TicketId))
            .ReturnsAsync(() => new Some<Ticket>(ticket));

        _ticketShelfService.Setup(s => s.ReserveTicket(ticket))
            .ReturnsAsync(() => new Ok());

        // when
        var result = await _sut.SellTicket(command);
        
        // then
        result.Should().BeOfType<Error>();
        _accountingService.Verify(s => s.TransferMoney(_buyer, _seller, _price), Times.Never);
        _ticketShelfService.Verify(s => s.SellTicket(ticket), Times.Never);
    }
    
    [Fact]
    public async Task ItShallNotSellTicketWithMoneyTransferFailed()
    {
        // given
        var ticket = new Ticket(_ticketToSell.TicketId, TicketState.Open);

        var command = new SellTicketCommand(_buyer, _seller, _ticketToSell, _price);

        _ticketRepository.Setup(r => r.FindById(_ticketToSell.TicketId))
            .ReturnsAsync(() => new Some<Ticket>(ticket));

        _ticketShelfService.Setup(s => s.ReserveTicket(ticket))
            .ReturnsAsync(() => new Ok());
        
        _accountingService.Setup(s => s.TransferMoney(_buyer, _seller, _price))
            .ReturnsAsync(() => new Error("Insufficient funds"));

        // when
        var result = await _sut.SellTicket(command);
        
        // then
        result.Should().BeOfType<Error>();
        _ticketShelfService.Verify(s => s.SellTicket(ticket), Times.Never);
    }
    
    [Fact]
    public async Task ItShallRollbackWhenTicketSellFailed()
    {
        // given
        var ticket = new Ticket(_ticketToSell.TicketId, TicketState.Open);

        var command = new SellTicketCommand(_buyer, _seller, _ticketToSell, _price);

        _ticketRepository.Setup(r => r.FindById(_ticketToSell.TicketId))
            .ReturnsAsync(() => new Some<Ticket>(ticket));

        _ticketShelfService.Setup(s => s.ReserveTicket(ticket))
            .ReturnsAsync(() => new Ok());
        
        _accountingService.Setup(s => s.TransferMoney(_buyer, _seller, _price))
            .ReturnsAsync(() => new Ok());

        _ticketShelfService.Setup(s => s.SellTicket(ticket))
            .ReturnsAsync(() => new Error("Unable to sell"));

        // when
        var result = await _sut.SellTicket(command);
        
        // then
        result.Should().BeOfType<Error>();
        _accountingService.Verify(s => s.RollBackTransfer(), Times.Once);
    }
}