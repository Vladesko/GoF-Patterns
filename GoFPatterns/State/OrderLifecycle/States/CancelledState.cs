namespace GoFPatterns.State.OrderLifecycle.States
{
    // Estado concreto: estado final.
    public class CancelledState : OrderStateBase
    {
        public override string Name => "Cancelado";
    }
}
