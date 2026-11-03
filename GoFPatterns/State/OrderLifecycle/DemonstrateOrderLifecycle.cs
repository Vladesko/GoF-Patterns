namespace GoFPatterns.State.OrderLifecycle
{
    public class DemonstrateOrderLifecycle
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== State: Ciclo di vita dell'ordine ===\n");

            Console.WriteLine("Percorso ideale:");
            var order = new Order(1, 150m);
            order.Ship();     // non ancora pagato
            order.Pay();
            order.Pay();      // già pagato
            order.Ship();
            order.Cancel();   // troppo tardi
            order.Deliver();

            Console.WriteLine("\nAnnullamento dopo il pagamento:");
            var cancelled = new Order(2, 80m);
            cancelled.Pay();
            cancelled.Cancel();
            cancelled.Ship();

            Console.WriteLine($"\nStati finali: #{order.Id} {order.State.Name}, #{cancelled.Id} {cancelled.State.Name}");
        }
    }
}
