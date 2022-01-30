using TicketSellService.Domain;
using TicketSellService.Domain.Model;
using TicketSellService.Domain.Ports;

namespace TicketSellService.Infrastructure.Adapters;

public class AccountingService : IAccountingService
{
    public Task<Result> TransferMoney(Buyer buyer, Seller seller, Price price)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RollBackTransfer()
    {
        throw new NotImplementedException();
    }
}