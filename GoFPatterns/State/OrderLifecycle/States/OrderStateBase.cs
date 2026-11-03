namespace GoFPatterns.State.OrderLifecycle.States
{
    // Базовое состояние - по умолчанию ни одно действие не разрешено.
    // Конкретные состояния переопределяют только те действия, которые для них имеют смысл.
    public abstract class OrderStateBase : IOrderState
    {
        public abstract string Name { get; }

        public virtual void Pay(Order order) => Deny(nameof(Pay));
        public virtual void Ship(Order order) => Deny(nameof(Ship));
        public virtual void Deliver(Order order) => Deny(nameof(Deliver));
        public virtual void Cancel(Order order) => Deny(nameof(Cancel));

        private void Deny(string action) =>
            Console.WriteLine($"    Действие {action} недоступно: заказ в состоянии \"{Name}\"");
    }
}
