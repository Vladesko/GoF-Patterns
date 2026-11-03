using GoFPatterns.Facade.OrderPlacement.Subsystems;

namespace GoFPatterns.Facade.OrderPlacement
{
    // Fachada: oculta todo el flujo del pedido: comprobaciones, reserva, pago, reversión, envío y notificación.
    // El controlador (cliente) solo llama a PlaceOrder y no depende de cuatro servicios.
    public class OrderFacade(
        InventoryService inventory,
        PaymentGateway payments,
        ShippingService shipping,
        NotificationService notifications)
    {
        public OrderResult PlaceOrder(OrderRequest order)
        {
            Console.WriteLine($"Realizando el pedido: {order.Count} x {order.Product}");

            if (!inventory.IsAvailable(order.Product, order.Count))
                return new OrderResult(false, $"{order.Product}: sin existencias");

            inventory.Reserve(order.Product, order.Count);

            var total = order.Price * order.Count + shipping.CalculateCost(order.Address);
            if (!payments.Charge(order.CardNumber, total))
            {
                // La fachada también sabe cómo revertir el estado del subsistema.
                inventory.Release(order.Product, order.Count);
                return new OrderResult(false, "El pago fue rechazado");
            }

            var tracking = shipping.CreateShipment(order.Product, order.Address);
            notifications.SendEmail(order.Email, $"El pedido está pagado ({total:F2}), número de seguimiento {tracking}");

            return new OrderResult(true, $"Pedido realizado, seguimiento {tracking}");
        }
    }
}
