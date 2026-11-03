namespace GoFPatterns.Facade.OrderPlacement
{
    // Datos de entrada de la fachada. El cliente lo pasa todo en un solo objeto.
    public record OrderRequest(string Product, int Count, decimal Price, string CardNumber, string Address, string Email);
}
