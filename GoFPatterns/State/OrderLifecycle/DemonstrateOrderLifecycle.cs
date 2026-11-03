namespace GoFPatterns.State.OrderLifecycle
{
    public class DemonstrateOrderLifecycle
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Состояние: жизненный цикл заказа ===\n");

            Console.WriteLine("Успешный сценарий:");
            var order = new Order(1, 150m);
            order.Ship();     // ещё не оплачен
            order.Pay();
            order.Pay();      // уже оплачен
            order.Ship();
            order.Cancel();   // слишком поздно
            order.Deliver();

            Console.WriteLine("\nОтмена после оплаты:");
            var cancelled = new Order(2, 80m);
            cancelled.Pay();
            cancelled.Cancel();
            cancelled.Ship();

            Console.WriteLine($"\nИтоговые состояния: #{order.Id} {order.State.Name}, #{cancelled.Id} {cancelled.State.Name}");
        }
    }
}
