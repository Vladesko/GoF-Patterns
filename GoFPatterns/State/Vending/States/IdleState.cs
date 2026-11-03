namespace GoFPatterns.State.Vending.States
{
    // Конкретное состояние - ожидание денег.
    public class IdleState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    баланс: {machine.Balance}");
            machine.SetState(VendingMachine.HasMoney);
        }

        public void SelectProduct(VendingMachine machine) =>
            Console.WriteLine($"    сначала внесите деньги, цена {machine.Price}");

        public void Refund(VendingMachine machine) =>
            Console.WriteLine("    нечего возвращать");
    }
}
