namespace GoFPatterns.Facade.OrderPlacement
{
    // Входные данные для фасада. Клиент передаёт всё в одном объекте.
    public record OrderRequest(string Product, int Count, decimal Price, string CardNumber, string Address, string Email);
}
