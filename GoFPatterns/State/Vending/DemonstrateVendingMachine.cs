namespace GoFPatterns.State.Vending
{
    public class DemonstrateVendingMachine
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== State: máquina expendedora ===\n");

            var machine = new VendingMachine(stock: 2, price: 50);

            void Act(Action action)
            {
                action();
                Console.WriteLine($"    [estado: {machine.StateName}]");
            }

            Act(machine.SelectProduct);
            Act(() => machine.InsertCoin(20));
            Act(machine.SelectProduct);
            Act(() => machine.InsertCoin(50));
            Act(machine.SelectProduct);

            Act(() => machine.InsertCoin(10));
            Act(machine.Refund);

            Act(() => machine.InsertCoin(50));
            Act(machine.SelectProduct);
            Act(() => machine.InsertCoin(50));
        }
    }
}
