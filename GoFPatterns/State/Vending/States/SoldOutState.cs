namespace GoFPatterns.State.Vending.States
{
    // Estado concreto: la máquina está vacía. El dinero se devuelve inmediatamente.
    public class SoldOutState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount) =>
            Console.WriteLine($"    agotado, se devuelve la moneda {amount}");

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine("    agotado");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    no hay nada que devolver");
    }
}
