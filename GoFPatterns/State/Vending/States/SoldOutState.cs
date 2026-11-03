namespace GoFPatterns.State.Vending.States
{
    // Konkreter Zustand – der Automat ist leer. Geld wird sofort zurückgegeben.
    public class SoldOutState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount) =>
            Console.WriteLine($"    ausverkauft, Münze {amount} wird zurückgegeben");

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine("    ausverkauft");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    nichts zurückzugeben");
    }
}
