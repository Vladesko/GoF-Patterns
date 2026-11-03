namespace GoFPatterns.State.OrderLifecycle
{
    public class DemonstrateOrderLifecycle
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== State: ciclo de vida del pedido ===\n");

            Console.WriteLine("Camino feliz:");
            var order = new Order(1, 150m);
            order.Ship();     // todavía no está pagado
            order.Pay();
            order.Pay();      // ya está pagado
            order.Ship();
            order.Cancel();   // demasiado tarde
            order.Deliver();

            Console.WriteLine("\nCancelación después del pago:");
            var cancelled = new Order(2, 80m);
            cancelled.Pay();
            cancelled.Cancel();
            cancelled.Ship();

            Console.WriteLine($"\nEstados finales: n.º {order.Id} {order.State.Name}, n.º {cancelled.Id} {cancelled.State.Name}");
        }
    }
}
