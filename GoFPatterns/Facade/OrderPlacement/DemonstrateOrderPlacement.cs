using GoFPatterns.Facade.OrderPlacement.Subsystems;

namespace GoFPatterns.Facade.OrderPlacement
{
    public class DemonstrateOrderPlacement
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Фасад: оформление заказа ===\n");

            var facade = new OrderFacade(new InventoryService(), new PaymentGateway(), new ShippingService(), new NotificationService());

            OrderRequest[] orders =
            [
                new("Ноутбук", 1, 1200m, "4111111111111111", "Москва, ул. Ленина, 1", "ivan@mail.com"),
                new("Телефон", 1, 700m, "4111111111111111", "Казань, ул. Баумана, 5", "olga@mail.com"),
                new("Ноутбук", 1, 1200m, "4111111111110000", "Казань, ул. Баумана, 5", "petr@mail.com")
            ];

            foreach (var order in orders)
            {
                var result = facade.PlaceOrder(order);
                Console.WriteLine($"=> {(result.Success ? "УСПЕХ" : "ОШИБКА")}: {result.Message}\n");
            }
        }
    }
}
