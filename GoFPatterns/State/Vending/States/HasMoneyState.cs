namespace GoFPatterns.State.Vending.States
{
    // Конкретное состояние - деньги внесены.
    public class HasMoneyState : IVendingState
    {
        public void InsertCoin(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"    баланс: {machine.Balance}");
        }

        public void SelectProduct(VendingMachine machine)
        {
            if (machine.Balance < machine.Price)
            {
                Console.WriteLine($"    недостаточно денег: {machine.Balance}/{machine.Price}");
                return;
            }

            machine.Balance -= machine.Price;
            machine.Stock--;
            Console.WriteLine($"    *товар выдан*, осталось товаров: {machine.Stock}");

            if (machine.Balance > 0)
            {
                Console.WriteLine($"    сдача: {machine.Balance}");
                machine.Balance = 0;
            }

            machine.SetState(machine.Stock > 0 ? VendingMachine.Idle : VendingMachine.SoldOut);
        }

        public void Refund(VendingMachine machine)
        {
            Console.WriteLine($"    возвращено {machine.Balance}");
            machine.Balance = 0;
            machine.SetState(VendingMachine.Idle);
        }
    }
}
