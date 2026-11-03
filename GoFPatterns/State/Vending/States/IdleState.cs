namespace GoFPatterns.State.Vending.States
{
    // Concrete state - waiting for money.
    public class IdleState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    balance: {machine.Balance}");
            machine.SetState(VendingMachine.HasMoney);
        }

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine($"    insert money first, price is {machine.Price}");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    nothing to refund");
    }
}
