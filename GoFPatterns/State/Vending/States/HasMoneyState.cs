namespace GoFPatterns.State.Vending.States
{
    // Konkreter Zustand – Geld wurde eingeworfen.
    public class HasMoneyState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    Guthaben: {machine.Balance}");
        }

        public void SelectProduct(VendingMachine machine)
        {
            if (machine.Balance < machine.Price)
            {
                Console.WriteLine($"    nicht genug Geld: {machine.Balance}/{machine.Price}");
                return;
            }

            machine.Balance -= machine.Price;
            machine.Stock--;
            Console.WriteLine($"    *Produkt fällt heraus*, verbleibende Artikel: {machine.Stock}");

            if (machine.Balance > 0)
            {
                Console.WriteLine($"    Wechselgeld: {machine.Balance}");
                machine.Balance = 0;
            }

            machine.SetState(machine.Stock > 0 ? VendingMachine.Idle : VendingMachine.SoldOut);
        }

        public void Refund(VendingMachine machine)
        {
            Console.WriteLine($"    {machine.Balance} zurückgegeben");
            machine.Balance = 0;
            machine.SetState(VendingMachine.Idle);
        }
    }
}
