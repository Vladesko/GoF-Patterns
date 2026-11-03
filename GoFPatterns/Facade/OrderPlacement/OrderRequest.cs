namespace GoFPatterns.Facade.OrderPlacement
{
    // Dati di input per la facade. Il client passa tutto in un unico oggetto.
    public record OrderRequest(string Product, int Count, decimal Price, string CardNumber, string Address, string Email);
}
