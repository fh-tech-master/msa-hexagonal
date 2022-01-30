using TicketSellService.Domain.Model;

namespace TicketSellService.Domain.Ports;

/// <summary>
/// Access to an accounting service
/// </summary>
public interface IAccountingService
{
    /// <summary>
    /// Transfers <paramref name="price"/> from <paramref name="buyer"/> to <paramref name="seller"/>
    /// </summary>
    /// <param name="buyer"></param>
    /// <param name="seller"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    Task<Result> TransferMoney(Buyer buyer, Seller seller, Price price);

    /// <summary>
    /// Rolls back a previous transfer
    /// </summary>
    /// <returns></returns>
    Task<Result> RollBackTransfer();
}