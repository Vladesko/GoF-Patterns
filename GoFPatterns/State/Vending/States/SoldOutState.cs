namespace GoFPatterns.State.Vending.States
{
    // Concrete state - machine is empty. Money is returned immediately.
    public class SoldOutState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount) =>
            Console.WriteLine($"    sold out, coin {amount} is returned");

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine("    sold out");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    nothing to refund");
    }
}
