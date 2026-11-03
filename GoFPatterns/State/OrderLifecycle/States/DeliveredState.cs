namespace GoFPatterns.State.OrderLifecycle.States
{
    // Estado concreto: estado final. La clase base deniega todas las acciones.
    public class DeliveredState : OrderStateBase
    {
        public override string Name => "Entregado";
    }
}
