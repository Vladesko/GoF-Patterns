namespace GoFPatterns.State.Vending.States
{
    // Stato concreto - il distributore è vuoto. Il denaro viene restituito subito.
    public class SoldOutState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount) =>
            Console.WriteLine($"    esaurito, la moneta da {amount} viene restituita");

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine("    esaurito");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    niente da restituire");
    }
}
