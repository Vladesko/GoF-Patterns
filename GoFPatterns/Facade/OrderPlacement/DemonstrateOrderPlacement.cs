using GoFPatterns.Facade.OrderPlacement.Subsystems;

namespace GoFPatterns.Facade.OrderPlacement
{
    public class DemonstrateOrderPlacement
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Facade: Order placement ===\n");

            var facade = new OrderFacade(new InventoryService(), new PaymentGateway(), new ShippingService(), new NotificationService());

            OrderRequest[] orders =
            [
                new("Laptop", 1, 1200m, "4111111111111111", "Moscow, Lenina 1", "ivan@mail.com"),
                new("Phone", 1, 700m, "4111111111111111", "Kazan, Baumana 5", "olga@mail.com"),
                new("Laptop", 1, 1200m, "4111111111110000", "Kazan, Baumana 5", "petr@mail.com")
            ];

            foreach (var order in orders)
            {
                var result = facade.PlaceOrder(order);
                Console.WriteLine($"=> {(result.Success ? "SUCCESS" : "FAILED")}: {result.Message}\n");
            }
        }
    }
}
