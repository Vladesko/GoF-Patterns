using GoFPatterns.Facade.OrderPlacement.Subsystems;

namespace GoFPatterns.Facade.OrderPlacement
{
    // Фасад - скрывает весь процесс оформления заказа: проверки, резервирование, оплату, откат, доставку, уведомление.
    // Контроллер (клиент) вызывает только PlaceOrder и не зависит от четырёх сервисов.
    public class OrderFacade(
        InventoryService inventory,
        PaymentGateway payments,
        ShippingService shipping,
        NotificationService notifications)
    {
        public OrderResult PlaceOrder(OrderRequest order)
        {
            Console.WriteLine($"Оформление заказа: {order.Count} x {order.Product}");

            if (!inventory.IsAvailable(order.Product, order.Count))
                return new OrderResult(false, $"{order.Product}: нет в наличии");

            inventory.Reserve(order.Product, order.Count);

            var total = order.Price * order.Count + shipping.CalculateCost(order.Address);
            if (!payments.Charge(order.CardNumber, total))
            {
                // Фасад также знает, как откатить состояние подсистем.
                inventory.Release(order.Product, order.Count);
                return new OrderResult(false, "Платёж отклонён");
            }

            var tracking = shipping.CreateShipment(order.Product, order.Address);
            notifications.SendEmail(order.Email, $"Заказ оплачен ({total:F2}), трек-номер {tracking}");

            return new OrderResult(true, $"Заказ оформлен, трек-номер {tracking}");
        }
    }
}
