using GoFPatterns.State.OrderLifecycle.States;

namespace GoFPatterns.State.OrderLifecycle
{
    // Contexto: el pedido. No contiene ningún "switch (status)";
    // simplemente delega cada acción en el objeto de estado actual.
    public class Order(int id, decimal amount)
    {
        public int Id { get; } = id;
        public decimal Amount { get; } = amount;
        public IOrderState State { get; private set; } = new NewState();

        // Los estados llaman a este método para pasar el pedido al siguiente estado.
        internal void TransitionTo(IOrderState state)
        {
            Console.WriteLine($"    Pedido n.º {Id}: {State.Name} -> {state.Name}");
            State = state;
        }

        public void Pay() => State.Pay(this);
        public void Ship() => State.Ship(this);
        public void Deliver() => State.Deliver(this);
        public void Cancel() => State.Cancel(this);
    }
}
