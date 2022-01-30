namespace TicketSellService.Domain;

public interface Optional<T>
{
    
}

public record Some<T>(T Value) : Optional<T>;
public record None<T>() : Optional<T>;