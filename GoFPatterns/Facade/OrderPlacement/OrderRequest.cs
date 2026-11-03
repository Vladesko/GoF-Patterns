namespace GoFPatterns.Facade.OrderPlacement
{
    // Input data for facade. Client passes everything in one object.
    public record OrderRequest(string Product, int Count, decimal Price, string CardNumber, string Address, string Email);
}
