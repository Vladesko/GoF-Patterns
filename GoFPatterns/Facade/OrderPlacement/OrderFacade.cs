using GoFPatterns.Facade.OrderPlacement.Subsystems;

namespace GoFPatterns.Facade.OrderPlacement
{
    // Facade - nasconde l'intero flusso dell'ordine: controlli, prenotazione, pagamento, rollback, spedizione, notifica.
    // Il controller (client) chiama solo PlaceOrder e non dipende da quattro servizi.
    public class OrderFacade(
        InventoryService inventory,
        PaymentGateway payments,
        ShippingService shipping,
        NotificationService notifications)
    {
        public OrderResult PlaceOrder(OrderRequest order)
        {
            Console.WriteLine($"Invio dell'ordine: {order.Count} x {order.Product}");

            if (!inventory.IsAvailable(order.Product, order.Count))
                return new OrderResult(false, $"{order.Product} non è disponibile in magazzino");

            inventory.Reserve(order.Product, order.Count);

            var total = order.Price * order.Count + shipping.CalculateCost(order.Address);
            if (!payments.Charge(order.CardNumber, total))
            {
                // La facade sa anche come ripristinare lo stato dei sottosistemi.
                inventory.Release(order.Product, order.Count);
                return new OrderResult(false, "Il pagamento è stato rifiutato");
            }

            var tracking = shipping.CreateShipment(order.Product, order.Address);
            notifications.SendEmail(order.Email, $"L'ordine è stato pagato ({total:F2}), numero di tracking {tracking}");

            return new OrderResult(true, $"Ordine effettuato, tracking {tracking}");
        }
    }
}
