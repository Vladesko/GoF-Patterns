namespace GoFPatterns.State.Vending
{
    // Состояние - реакция автомата на действия пользователя.
    public interface IVendingState
    {
        void InsertCoin(VendingMachine machine, int amount);
        void SelectProduct(VendingMachine machine);
        void Refund(VendingMachine machine);
    }
}
