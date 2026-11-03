namespace GoFPatterns.State.Vending.States
{
    // Konkreter Zustand – wartet auf Geld.
    public class IdleState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    Guthaben: {machine.Balance}");
            machine.SetState(VendingMachine.HasMoney);
        }

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine($"    bitte zuerst Geld einwerfen, der Preis beträgt {machine.Price}");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    nichts zurückzugeben");
    }
}
