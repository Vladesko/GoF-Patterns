namespace GoFPatterns.State.Vending.States
{
    // Estado concreto: esperando dinero.
    public class IdleState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    saldo: {machine.Balance}");
            machine.SetState(VendingMachine.HasMoney);
        }

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine($"    primero introduzca dinero, el precio es {machine.Price}");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    no hay nada que devolver");
    }
}
