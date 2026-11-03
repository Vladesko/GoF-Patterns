using GoFPatterns.Facade.OrderPlacement.Subsystems;

namespace GoFPatterns.Facade.OrderPlacement
{
    // Facade - hides the whole order workflow: checks, reservation, payment, rollback, shipping, notification.
    // Controller (client) calls only PlaceOrder and doesn't depend on four services.
    public class OrderFacade(
        InventoryService inventory,
        PaymentGateway payments,
        ShippingService shipping,
        NotificationService notifications)
    {
        public OrderResult PlaceOrder(OrderRequest order)
        {
            Console.WriteLine($"Placing order: {order.Count} x {order.Product}");

            if (!inventory.IsAvailable(order.Product, order.Count))
                return new OrderResult(false, $"{order.Product} is out of stock");

            inventory.Reserve(order.Product, order.Count);

            var total = order.Price * order.Count + shipping.CalculateCost(order.Address);
            if (!payments.Charge(order.CardNumber, total))
            {
                // Facade also knows how to roll back subsystem state.
                inventory.Release(order.Product, order.Count);
                return new OrderResult(false, "Payment was declined");
            }

            var tracking = shipping.CreateShipment(order.Product, order.Address);
            notifications.SendEmail(order.Email, $"Order is paid ({total:F2}), tracking number {tracking}");

            return new OrderResult(true, $"Order placed, tracking {tracking}");
        }
    }
}
