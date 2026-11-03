using GoFPatterns.Facade.OrderPlacement.Subsystems;

namespace GoFPatterns.Facade.OrderPlacement
{
    // Fassade – verbirgt den gesamten Bestellablauf: Prüfungen, Reservierung, Zahlung, Rollback, Versand, Benachrichtigung.
    // Der Controller (Client) ruft nur PlaceOrder auf und hängt nicht von vier Diensten ab.
    public class OrderFacade(
        InventoryService inventory,
        PaymentGateway payments,
        ShippingService shipping,
        NotificationService notifications)
    {
        public OrderResult PlaceOrder(OrderRequest order)
        {
            Console.WriteLine($"Bestellung wird aufgegeben: {order.Count} x {order.Product}");

            if (!inventory.IsAvailable(order.Product, order.Count))
                return new OrderResult(false, $"{order.Product} ist nicht vorrätig");

            inventory.Reserve(order.Product, order.Count);

            var total = order.Price * order.Count + shipping.CalculateCost(order.Address);
            if (!payments.Charge(order.CardNumber, total))
            {
                // Die Fassade weiß auch, wie der Zustand der Subsysteme zurückgesetzt wird.
                inventory.Release(order.Product, order.Count);
                return new OrderResult(false, "Zahlung wurde abgelehnt");
            }

            var tracking = shipping.CreateShipment(order.Product, order.Address);
            notifications.SendEmail(order.Email, $"Bestellung ist bezahlt ({total:F2}), Sendungsnummer {tracking}");

            return new OrderResult(true, $"Bestellung aufgegeben, Sendungsnummer {tracking}");
        }
    }
}
