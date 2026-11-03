namespace GoFPatterns.State.Vending.States
{
    // Stato concreto - il denaro è stato inserito.
    public class HasMoneyState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    credito: {machine.Balance}");
        }

        public void SelectProduct(VendingMachine machine)
        {
            if (machine.Balance < machine.Price)
            {
                Console.WriteLine($"    denaro insufficiente: {machine.Balance}/{machine.Price}");
                return;
            }

            machine.Balance -= machine.Price;
            machine.Stock--;
            Console.WriteLine($"    *prodotto erogato*, articoli rimasti: {machine.Stock}");

            if (machine.Balance > 0)
            {
                Console.WriteLine($"    resto: {machine.Balance}");
                machine.Balance = 0;
            }

            machine.SetState(machine.Stock > 0 ? VendingMachine.Idle : VendingMachine.SoldOut);
        }

        public void Refund(VendingMachine machine)
        {
            Console.WriteLine($"    restituito {machine.Balance}");
            machine.Balance = 0;
            machine.SetState(VendingMachine.Idle);
        }
    }
}
