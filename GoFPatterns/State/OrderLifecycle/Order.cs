using GoFPatterns.State.OrderLifecycle.States;

namespace GoFPatterns.State.OrderLifecycle
{
    // Контекст - заказ. В нём нигде нет "switch (status)",
    // он просто делегирует каждое действие текущему объекту состояния.
    public class Order(int id, decimal amount)
    {
        public int Id { get; } = id;
        public decimal Amount { get; } = amount;
        public IOrderState State { get; private set; } = new NewState();

        // Состояния вызывают этот метод, чтобы перевести заказ в следующее состояние.
        internal void TransitionTo(IOrderState state)
        {
            Console.WriteLine($"    Заказ #{Id}: {State.Name} -> {state.Name}");
            State = state;
        }

        public void Pay() => State.Pay(this);
        public void Ship() => State.Ship(this);
        public void Deliver() => State.Deliver(this);
        public void Cancel() => State.Cancel(this);
    }
}
