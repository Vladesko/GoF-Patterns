namespace GoFPatterns.State.OrderLifecycle
{
    public class DemonstrateOrderLifecycle
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== State: Lebenszyklus einer Bestellung ===\n");

            Console.WriteLine("Normalfall (Happy Path):");
            var order = new Order(1, 150m);
            order.Ship();     // noch nicht bezahlt
            order.Pay();
            order.Pay();      // bereits bezahlt
            order.Ship();
            order.Cancel();   // zu spät
            order.Deliver();

            Console.WriteLine("\nStornierung nach der Zahlung:");
            var cancelled = new Order(2, 80m);
            cancelled.Pay();
            cancelled.Cancel();
            cancelled.Ship();

            Console.WriteLine($"\nEndzustände: Nr. {order.Id} {order.State.Name}, Nr. {cancelled.Id} {cancelled.State.Name}");
        }
    }
}
