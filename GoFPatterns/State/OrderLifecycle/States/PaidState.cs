namespace GoFPatterns.State.OrderLifecycle.States
{
    // Конкретное состояние - заказ оплачен. Отмена здесь означает возврат денег.
    public class PaidState : OrderStateBase
    {
        public override string Name => "Оплачен";

        public override void Ship(Order order)
        {
            Console.WriteLine("    Посылка передана курьеру");
            order.TransitionTo(new ShippedState());
        }

        public override void Cancel(Order order)
        {
            Console.WriteLine($"    Возврат {order.Amount:F2} покупателю");
            order.TransitionTo(new CancelledState());
        }
    }
}
