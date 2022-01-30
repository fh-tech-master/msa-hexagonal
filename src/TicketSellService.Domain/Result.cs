namespace TicketSellService.Domain;

public interface Result
{
    
}

public record Ok() : Result;

public record Error(string Message) : Result;
