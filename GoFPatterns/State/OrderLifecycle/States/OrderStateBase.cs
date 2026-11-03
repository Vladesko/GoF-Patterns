namespace GoFPatterns.State.OrderLifecycle.States
{
    // Stato base - di default nessuna azione è consentita.
    // Gli stati concreti ridefiniscono solo le azioni che per loro hanno senso.
    public abstract class OrderStateBase : IOrderState
    {
        public abstract string Name { get; }

        public virtual void Pay(Order order) => Deny("pagare");
        public virtual void Ship(Order order) => Deny("spedire");
        public virtual void Deliver(Order order) => Deny("consegnare");
        public virtual void Cancel(Order order) => Deny("annullare");

        private void Deny(string action) =>
            Console.WriteLine($"    Impossibile {action.ToLower()}: l'ordine è {Name}");
    }
}
