namespace GoFPatterns.State.Vending.States
{
    // Stato concreto - in attesa di denaro.
    public class IdleState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    credito: {machine.Balance}");
            machine.SetState(VendingMachine.HasMoney);
        }

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine($"    inserisci prima il denaro, il prezzo è {machine.Price}");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    niente da restituire");
    }
}
