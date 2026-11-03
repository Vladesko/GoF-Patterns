namespace GoFPatterns.State.OrderLifecycle.States
{
    // Конкретное состояние - конечное состояние.
    public class CancelledState : OrderStateBase
    {
        public override string Name => "Отменён";
    }
}
