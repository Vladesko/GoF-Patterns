namespace GoFPatterns.State.OrderLifecycle.States
{
    // Конкретное состояние - заказ в пути. Отменить его уже нельзя.
    public class ShippedState : OrderStateBase
    {
        public override string Name => "Отправлен";

        public override void Deliver(Order order)
        {
            Console.WriteLine("    Покупатель получил посылку");
            order.TransitionTo(new DeliveredState());
        }
    }
}
