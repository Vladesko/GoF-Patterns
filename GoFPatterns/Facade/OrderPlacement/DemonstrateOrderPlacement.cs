using GoFPatterns.Facade.OrderPlacement.Subsystems;

namespace GoFPatterns.Facade.OrderPlacement
{
    public class DemonstrateOrderPlacement
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Facade: Invio dell'ordine ===\n");

            var facade = new OrderFacade(new InventoryService(), new PaymentGateway(), new ShippingService(), new NotificationService());

            OrderRequest[] orders =
            [
                new("Portatile", 1, 1200m, "4111111111111111", "Mosca, via Lenin 1", "ivan@mail.com"),
                new("Telefono", 1, 700m, "4111111111111111", "Kazan, via Bauman 5", "olga@mail.com"),
                new("Portatile", 1, 1200m, "4111111111110000", "Kazan, via Bauman 5", "petr@mail.com")
            ];

            foreach (var order in orders)
            {
                var result = facade.PlaceOrder(order);
                Console.WriteLine($"=> {(result.Success ? "SUCCESSO" : "FALLITO")}: {result.Message}\n");
            }
        }
    }
}
