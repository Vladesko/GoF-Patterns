namespace GoFPatterns.State.OrderLifecycle.States
{
    // Конкретное состояние - конечное состояние. Все действия запрещены базовым классом.
    public class DeliveredState : OrderStateBase
    {
        public override string Name => "Доставлен";
    }
}
