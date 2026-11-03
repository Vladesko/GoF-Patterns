namespace GoFPatterns.State.OrderLifecycle
{
    public class DemonstrateOrderLifecycle
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== State: Order lifecycle ===\n");

            Console.WriteLine("Happy path:");
            var order = new Order(1, 150m);
            order.Ship();     // not paid yet
            order.Pay();
            order.Pay();      // already paid
            order.Ship();
            order.Cancel();   // too late
            order.Deliver();

            Console.WriteLine("\nCancel after payment:");
            var cancelled = new Order(2, 80m);
            cancelled.Pay();
            cancelled.Cancel();
            cancelled.Ship();

            Console.WriteLine($"\nFinal states: #{order.Id} {order.State.Name}, #{cancelled.Id} {cancelled.State.Name}");
        }
    }
}
