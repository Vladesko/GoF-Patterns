namespace GoFPatterns.Facade.OrderPlacement
{
    // Eingabedaten für die Fassade. Der Client übergibt alles in einem Objekt.
    public record OrderRequest(string Product, int Count, decimal Price, string CardNumber, string Address, string Email);
}
