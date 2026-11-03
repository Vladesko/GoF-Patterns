namespace GoFPatterns.State.Vending.States
{
    // Конкретное состояние - автомат пуст. Деньги сразу возвращаются.
    public class SoldOutState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount) =>
            Console.WriteLine($"    товар закончился, монета {amount} возвращена");

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine("    товар закончился");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    нечего возвращать");
    }
}
