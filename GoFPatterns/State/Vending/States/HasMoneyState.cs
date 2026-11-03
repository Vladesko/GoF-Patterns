namespace GoFPatterns.State.Vending.States
{
    // Concrete state - money is inserted.
    public class HasMoneyState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    balance: {machine.Balance}");
        }

        public void SelectProduct(VendingMachine machine)
        {
            if (machine.Balance < machine.Price)
            {
                Console.WriteLine($"    not enough money: {machine.Balance}/{machine.Price}");
                return;
            }

            machine.Balance -= machine.Price;
            machine.Stock--;
            Console.WriteLine($"    *product dropped*, items left: {machine.Stock}");

            if (machine.Balance > 0)
            {
                Console.WriteLine($"    change: {machine.Balance}");
                machine.Balance = 0;
            }

            machine.SetState(machine.Stock > 0 ? VendingMachine.Idle : VendingMachine.SoldOut);
        }

        public void Refund(VendingMachine machine)
        {
            Console.WriteLine($"    returned {machine.Balance}");
            machine.Balance = 0;
            machine.SetState(VendingMachine.Idle);
        }
    }
}
