namespace GoFPatterns.State.Vending.States
{
    // Estado concreto: se ha introducido dinero.
    public class HasMoneyState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    saldo: {machine.Balance}");
        }

        public void SelectProduct(VendingMachine machine)
        {
            if (machine.Balance < machine.Price)
            {
                Console.WriteLine($"    dinero insuficiente: {machine.Balance}/{machine.Price}");
                return;
            }

            machine.Balance -= machine.Price;
            machine.Stock--;
            Console.WriteLine($"    *producto dispensado*, quedan {machine.Stock} unidades");

            if (machine.Balance > 0)
            {
                Console.WriteLine($"    cambio: {machine.Balance}");
                machine.Balance = 0;
            }

            machine.SetState(machine.Stock > 0 ? VendingMachine.Idle : VendingMachine.SoldOut);
        }

        public void Refund(VendingMachine machine)
        {
            Console.WriteLine($"    devuelto {machine.Balance}");
            machine.Balance = 0;
            machine.SetState(VendingMachine.Idle);
        }
    }
}
