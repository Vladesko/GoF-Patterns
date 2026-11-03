namespace GoFPatterns.State.OrderLifecycle.States
{
    // Конкретное состояние - заказ создан, но ещё не оплачен.
    public class NewState : OrderStateBase
    {
        public override string Name => "Новый";

        public override void Pay(Order order)
        {
            Console.WriteLine($"    Оплата {order.Amount:F2} принята");
            order.TransitionTo(new PaidState());
        }

        public override void Cancel(Order order) => order.TransitionTo(new CancelledState());
    }
}
