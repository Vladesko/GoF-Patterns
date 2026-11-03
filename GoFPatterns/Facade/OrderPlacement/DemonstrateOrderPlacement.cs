using GoFPatterns.Facade.OrderPlacement.Subsystems;

namespace GoFPatterns.Facade.OrderPlacement
{
    public class DemonstrateOrderPlacement
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Facade: realización de pedidos ===\n");

            var facade = new OrderFacade(new InventoryService(), new PaymentGateway(), new ShippingService(), new NotificationService());

            OrderRequest[] orders =
            [
                new("Portátil", 1, 1200m, "4111111111111111", "Moscow, Lenina 1", "ivan@mail.com"),
                new("Teléfono", 1, 700m, "4111111111111111", "Kazan, Baumana 5", "olga@mail.com"),
                new("Portátil", 1, 1200m, "4111111111110000", "Kazan, Baumana 5", "petr@mail.com")
            ];

            foreach (var order in orders)
            {
                var result = facade.PlaceOrder(order);
                Console.WriteLine($"=> {(result.Success ? "ÉXITO" : "FALLO")}: {result.Message}\n");
            }
        }
    }
}
